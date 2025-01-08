
color0	equ $2fc
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

mypmbase	equ $1600

	run $2000
	org $2000
	lda:cmp:req 20 ;pause 1
	sei
	mva #$00 nmien 
	mva #$fe portb	;turn off osrom and basicrom
	mwa #NMI $fffa		

	mwa #gameDli.dli dli_ptr ;vdslst
	mwa #gameVbi.vbi vbi_ptr
	mva #1+12+32 dmactl ;d400 = 559
	mva #2 gractl	;enable Players
	mva >mypmbase pmbase
	mwa #ingame_dl dlistl
	
.rept	8,#
	mva colors+:1 gamedli.c:1
	mva colors+:1 gamedli.dli2.c:1
.endr
	
	mva #$c0 nmien ;c0,40
	
	mva #26 count
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
	
	jmp *
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

/*
	ldy #>vramfont+4*?i
@	ldx #4*4
@	lda c2:#$1e
	sta wsync
	sta colpf0
	ift debug_leveldata=0
	sty chbase
	eif
	mva c4:#$26 colpf0+2
	mva c6:#$76 colpf0+3
	;mva #$00 colpf0+4
	
	lda c3:#$14
	sta wsync
	sta colpf0
	mva c5:#$6a colpf0+2
	mva c7:#$b8 colpf0+3
	*/


ingame_dl
	dta $70,$70,$60
	dta $80 ;f
	dta $44
addr	dta a(vram)
	
:23	dta $4
	dta $0
	dta $41,a(ingame_dl)
	
	
NMI	bit nmist
	bpl nmi_vbi	;vbi
	jmp (dli_ptr)	;dli
nmi_vbi	jmp (vbi_ptr)


	org mypmbase
	ins 'pmdata.dat'
	org $4000
vramfont	ins 'font.fnt'	
vrfont2	org $6000

	org $8000
vram	
	ins 'vram.dat'

;pf0-pf3 of line0 , pf0-pf3 of line1 , background, pmg (same for pm0-pm3)	
colors	ins 'colors2.dat'
	/*dta $0e,$00 ;static
	dta $1a,$14
	dta $26,$48
	dta $0a,$ba*/
ilace	equ *-1
;dta 1 ;use interlace	

	icl "matosimi_macros.asx"
	
