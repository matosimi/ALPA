;NTSC support

color0	equ $2c4
hposp0	equ $d000
hposm0	equ $d004
sizep0	equ $d008
sizem	equ $d00c
trig0	equ $d010
colpm0	equ $d012
colpf0	equ $d016
colbk	equ $d01a
prior	equ $d01b
vdelay	equ $d01c ;shift PM by 1 scanline, first missiles,then players(bits)
gractl	equ $d01d ;BIT1-ACTIV.PMG
consol	equ $d01f
random	equ $d20a
skctl	equ $d20f
porta	equ $d300 ;stick 0,1
portb	equ $d301
dmactl	equ $d400
dlistl	equ $d402
hscrol	equ $d404
pmbase	equ $d407
chbase	equ $d409
wsync	equ $d40a
vcount	equ $d40b
nmien	equ $d40e
nmist	equ $d40f

roto	equ $40


dli_ptr	equ $80
vbi_ptr	equ $82
w1	equ $84	;2b
ll0	equ $86	;last leading 0
ft0	equ $87	;first trailing 0
w2	equ $88	;2b
ll1	equ $8a
ft1	equ $8b
token	equ $8c	;zx7
lenL	equ $8d	;zx7
w3	equ $8e	;2b
w4	equ $90	;2b
tmp	equ $92
p0x	equ $93	;player positions (in px without offset to visible area)
p0y	equ $94
p1x	equ $95
p1y	equ $96
stick	equ $97
ntsc	equ $98
ntsctimer	equ $99
ntsccolor	equ $9a

mypmbase	equ $1600

	run start

	org $2000
start	;detect video system
.local
	mva #0 ntsc
	sta ntsctimer
	ldx 20
	inx
	inx
x1	lda vcount
	a_lt ntsc x2
	sta ntsc
x2	cpx 20
	bne x1
	lda ntsc
	a_lt #140 sys_ntsc
	mva #1 ntsc
	getcolor.setpal
sys_ntsc
.endl	
	sei
	mva #$00 nmien 
	mva #$fe portb	;turn off osrom and basicrom
	mwa #NMI $fffa		
	mva #1 580	;boot on reset

	mwa #gameDli.dli dli_ptr ;vdslst
	mwa #gameVbi.vbi vbi_ptr
	mva #0 dmactl ;d400 = 559 blackout screen
	mva #2 gractl	;enable Players
	mva >mypmbase pmbase
	mwa #ingame_dl dlistl
	
.rept	8,#
	lda colors+:1 
	getcolor
	sta gamedli.c:1
	sta gamedli.dli2.c:1
.endr

	mva #24 count
@	ldx #0
@
x1	lda vramfont,x
x2	sta vrfont2+1,x
x3	lda vramfont+1,x
x4	sta vrfont2,x
	inx
	inx
	bne @-	
	inc x1+2
	inc x2+2
	inc x3+2
	inc x4+2
	dec count
	bne @-1
	
	;sync the screen end
@	lda vcount
	a_lt #$80 @-
	
	mva #1+12+32 dmactl ;d400 = 559
	mva #$c0 nmien ;c0,40

@	lda consol
	cmp #6	
	bne @-
	rts
	
count	dta 0
	
	
.local gameVbi	
vbi	phr
	inc 20
	mva colors+8 colpf0+4 ;bg
	mva #$04 prior
:4	mva #64+32*:1 hposp0+:1
	lda colors+9
:4	sta colpm0+:1
	lda #3
:4	sta sizep0+:1
	plr
	rti
.endl
	
.local gamedli	
dli	phr
	mva #6 $c0
	ldy poop:#>vramfont
@	ldx #4*4
@	lda c0:#$1a
	sta wsync
	sta colpf0
	sty chbase
	mva c2:#$26 colpf0+2
	mva c3:#$0a colpf0+3
	mva c1:#$b6 colpf0+1
	
	lda c4:#$14
	sta wsync
	sta colpf0
	mva c6:#$48 colpf0+2
	mva c7:#$ba colpf0+3
	mva c5:#$ba colpf0+1
	;mva random colpf0
	dex
	bne @-
	
	iny
	iny
	iny
	iny
	dec $c0
	bne @-1
	lda ilace
	beq @+
	mwa #gameDli.dli2 dli_ptr
@	plr
	rti

.local dli2
	phr

	mva #6 $c0
	ldy #>vrfont2
@	ldx #4*4
@	lda c4:#$1a
	sta wsync
	sta colpf0
	sty chbase
	mva c6:#$26 colpf0+2
	mva c7:#$0a colpf0+3
	mva c5:#$04 colpf0+1
	
	lda c0:#$14
	sta wsync
	sta colpf0
	mva c2:#$48 colpf0+2
	mva c3:#$ba colpf0+3
	mva c1:#$02 colpf0+1
	dex
	bne @-
	
	iny
	iny
	iny
	iny
	dec $c0
	bne @-1
	
	mwa #gameDli.dli dli_ptr
	
	plr
	rti
.endl
.endl
	
	
NMI	bit nmist
	bpl nmi_vbi	;vbi
	jmp (dli_ptr)	;dli
nmi_vbi	jmp (vbi_ptr)

;128 color table that matches PAL colors to NTSC
ntsccolors128
:8	dta #*2
:8	dta #*2+$20	;$10
:8	dta #*2+$30
:8	dta #*2+$40
:8	dta #*2+$50
:8	dta #*2+$60
:8	dta #*2+$70
:8	dta #*2+$80
:8	dta #*2+$90	;$80
:8	dta #*2+$a0	;$90 pal
:8	dta #*2+$b0	;$a0 pal
:5	dta #*2+$d0+2	;$b0 pal
:3	dta #*2+$d0+$a	;$b0 pal second part
:4	dta #*2+$e0+2	;$c0 pal
:4	dta #*2+$e0+8	;$c0 pal second part
:4	dta #*2+$12	;$d0 pal
	dta $e8,$1a,$1c,$1e	;$d0 pal second part 
:8	dta #*2+$f0	;$e0 pal
:8	dta #*2+$20	;$f0 pal	

;returns ntsc color in A based on input pal color (in case ntsc is used)
.proc	getcolor (.byte a) .reg
pptr	lsr @
	stx ntsccolor	;save X-register for DLI calls
	tax
	lda ntsccolors128,x
	ldx ntsccolor
	rts

.proc	setpal
	mva #{rts} pptr
	rts	
.endp
.endp

.print "code: $2000-",*

	org mypmbase
	ins 'pmdata.dat'
	org $4000
vramfont	ins 'font.fnt'	
.print "inserted font:",vramfont,"-",*-1
vrfont2	;org $6000
.print "calculated font:",*,"-",*+*-vramfont-1

	org *+*-vramfont ;$8000
	.align $1000
vram	
	ins 'vram.dat'

;pf0-pf3 of line0 , pf0-pf3 of line1 , background, pmg (same for pm0-pm3)	
colors	ins 'colors2.dat'
;colors	ins 'colors2.dat'
	/*dta $0e,$00 ;static
	dta $1a,$14
	dta $26,$48
	dta $0a,$ba*/
ilace	equ *-1
.print "vram&colors:",vram,"-",*-1

ingame_dl
	dta $70,$70,$60
	dta $80 ;f
	dta $44
addr	dta a(vram)
	
:23	dta $4
	dta $0
	dta $41,a(ingame_dl)

;dta 1 ;use interlace	


	icl "matosimi_macros.asx"
	
