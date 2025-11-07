;NTSC support

sdmctl	equ $22f	;559
coldst	equ $244	;580, set to 1 to cold start on RESET
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
irqen	equ $d20e	;0 = disable all maskable IRQs (keyboard,break,...)
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

pokmsk	equ $10	;irq masking shadow
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
zpa	equ $9d	;DLI
zpx	equ $9e
zpy	equ $9f

mypmbase	equ $1600

	run start

	org $2000
	
.local	init
	mva #0 irqen	;disable key and break interrupts
	sta pokmsk	;shadow irqen
	mva #1 coldst	;cold start after pressing reset btn.
	mwa #idl $230
	;mva #$32 color0+3
	mva #0 sdmctl	;559
	mva #$ff portb ;turn on osrom a load next block
	rts
idl	dta $70
/*,$70,$70,$48,a(gr3)
:23	dta 8
	dta $70
	dta 2*/
	dta $41,a(idl)
/*	
gr3	dta 1 ; insert gr3 loading pic here
:256	dta $1*/
last
.endl
	ini init
	
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
	mva #3 gractl ;2	;enable Players
	mva #$00 vdelay
	mva >mypmbase pmbase
	mwa #ingame_dl dlistl
	
	set_kernel_colors2
	
	mva #38 count
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
	
	mva #2+12+32 dmactl ;d400 = 559
	mva #$c0 nmien ;c0,40

@	lda consol
	cmp #6	
	bne @-
	rts
	
count	dta 0
	
	
.local gameVbi	
vbi	phr
	inc 20
	mva colors colpf0+4 ;bg
	mva #$04 prior
:4	mva #48+32*:1 hposp0+:1
:4	mva #48+32*4+8*(3-:1) hposm0+:1
	lda colors+1 ;pmgs
:4	sta colpm0+:1
	lda #3
:4	sta sizep0+:1
	mva #$ff sizem
;:4	mva zpcolor+# colpf0+#
	
@

	
	plr
	rti
.endl
	
.local gamedli
dli	
	sta zpa
	stx zpx
	sty zpy

?newfont = 0
/*	
.rept 10,#
?set = :1*4	;charset number
.rept 12,#
?dsc = :1		;double scan lines*/
.rept 10*12,#
?set = (:1 / 12)*4	;charset number
?dsc = (:1 % 12)	;double scan lines

	lda c0:1:#$0f ; 26 ;colors
	ldx c1:1:#$12 ;colors+1
	ldy c2:1:#$28 ;colors+2

	ift ?newfont == 0
		ift ?set == 0	;very first font
:20			nop
		els
:17			nop	;all other new fonts
		eif

	els
	sta wsync
	eif
	
	sta colpf0
	ift ?newfont == 0
	mva #>vramfont+?set chbase
	eif
	stx colpf0+1 ;colors+1 colpf0+1
	sty colpf0+2
	mva c3:1:#$92 colpf0+3 

	lda c4:1:#$14 ;colors+4
	ldx c5:1:#$a4 ;colors+5
	ldy c6:1:#$a8 ;colors+6
	ift ?newfont > 0
		ift ?dsc % 4 == 0
;:6		nop
		els
		sta wsync
		eif
	eif
	sta colpf0
	stx colpf0+1 ;colors+5 colpf0+1
	sty colpf0+2
	mva c7:1:#$10 colpf0+3 ;colors+7 
?newfont++	
	ift ?newfont == 12
	?newfont = 0
	eif 
.endr

	mwa #gameDli.dlib dli_ptr
	lda zpa
	ldx zpx
	ldy zpy
	rti

	
dlib	

	sta zpa
	stx zpx
	sty zpy

?newfont = 0	
.rept 10*12,#
?set = (:1 / 12)*4	;charset number
?dsc = (:1 % 12)	;double scan lines

	lda c4b:1:#$14 ;colors+4
	ldx c5b:1:#$a4 ;colors+5
	ldy c6b:1:#$a8 ;colors+6
	
	

	ift ?newfont == 0
		ift ?set == 0	;very first font
:20			nop
		els
:17			nop	;all other new fonts
		eif

	els
	sta wsync
	eif
	
	sta colpf0
	ift ?newfont == 0
	mva #>vrfont2+?set chbase
	eif
	stx colpf0+1 ;colors+1 colpf0+1
	sty colpf0+2
	
	mva c7b:1:#$10 colpf0+3 ;colors+7 
	lda c0b:1:#$26 ;colors
	ldx c1b:1:#$12 ;colors+1
	ldy c2b:1:#$28 ;colors+2
	ift ?newfont > 0
		ift ?dsc % 4 == 0
;:6		nop
		els
		sta wsync
		eif
	eif
	sta colpf0
	stx colpf0+1 ;colors+5 colpf0+1
	sty colpf0+2
	mva c3b:1:#$92 colpf0+3
?newfont++	
	ift ?newfont == 12
	?newfont = 0
	eif 
.endr
	mwa #gameDli.dli dli_ptr
	lda zpa
	ldx zpx
	ldy zpy
	rti

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

ctab0
:12*10 dta a(gamedli.c0:1)
ctab1
:12*10 dta a(gamedli.c1:1)
ctab2
:12*10 dta a(gamedli.c2:1)
ctab3
:12*10 dta a(gamedli.c3:1)
ctab4
:12*10 dta a(gamedli.c4:1)
ctab5
:12*10 dta a(gamedli.c5:1)
ctab6
:12*10 dta a(gamedli.c6:1)
ctab7
:12*10 dta a(gamedli.c7:1)

ctab0b
:12*10 dta a(gamedli.c0b:1)
ctab1b
:12*10 dta a(gamedli.c1b:1)
ctab2b
:12*10 dta a(gamedli.c2b:1)
ctab3b
:12*10 dta a(gamedli.c3b:1)
ctab4b
:12*10 dta a(gamedli.c4b:1)
ctab5b
:12*10 dta a(gamedli.c5b:1)
ctab6b
:12*10 dta a(gamedli.c6b:1)
ctab7b
:12*10 dta a(gamedli.c7b:1)

tables
:8	dta a(ctab:1)
:8	dta a(ctab:1b)

.proc	set_kernel_colors2
	ldy #0	;not changing
	mva #7 colocount	;8 colors
@	ldx #120	;120 occurences
	
@	get_source_set_target
	dex
	bne @-

	dec colocount
	bpl @-1
	rts
	
.proc	get_source_set_target
	lda cp1:colors+2
	inw cp1
	getcolor
	sta value_to_store
	
	lda tp0:ctab0
	sta w1
	lda tp1:ctab0+1
	sta w1+1
	lda tp2:ctab0b
	sta w2
	lda tp3:ctab0b+1
	sta w2+1
	lda value_to_store:#00
	sta (w1),y
	sta (w2),y
	
	add16 #2 tp0
	add16 #2 tp1
	add16 #2 tp2
	add16 #2 tp3
	rts
.endp
colocount	dta 0

.endp

.print "code: $2000-",*

	org mypmbase-8
	ins 'pmdata.dat',0,4*128
	org mypmbase-8-128
	ins 'pmdata.dat',4*128,128
	org $6000
vramfont	ins 'font.fnt'	
.print "inserted font:",vramfont,"-",*-1
	ift *<vramfont+1024*10
:vramfont+1024*10-*	dta 0
	eif
vrfont2	;org vramfont+1024*10 ;org $6000
.print "calculated font:",*,"-",*+*-vramfont-1

	org *+*-vramfont ;$8000
	.align $1000
vram	
	ins 'vram.dat'

;pf0-pf3 of line0 , pf0-pf3 of line1 , background, pmg (same for pm0-pm3)	
colors	ins 'colors3.dat' ;background, PMG, and then color0,color1,color2,color3 for each line


ilace	equ *-1
.print "vram&colors:",vram,"-",*-1

ingame_dl
	dta $f0
	dta $44
addr	dta a(vram)

:27	dta 4
	dta $0
	dta $41,a(ingame_dl)

;dta 1 ;use interlace	


	icl "matosimi_macros.asx"
	
