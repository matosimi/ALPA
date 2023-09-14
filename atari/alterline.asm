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

dli_ptr	equ $80
vbi_ptr	equ $82
w1	equ $84
w2	equ $86


	run $2000
	org $2000
	lda:cmp:req 20 ;pause 1
	sei
	mva #$00 nmien 
	mva #$fe portb	;turn off osrom and basicrom
	mwa #NMI $fffa		

	mwa #gameDli.dli dli_ptr ;vdslst
	mwa #gameVbi.vbi vbi_ptr
	mva #1+12+16+32 dmactl ;d400 = 559
	mwa #ingame_dl dlistl
	
	mva #$c0 nmien ;c0
	jmp *
	
	
.local gameVbi	
vbi	phr
	mva #>gamefont chbase
	mva #$16 colpf0+1
	mva #$26 colpf0+2
	mva #$00 colpf0+4
	plr
	rti
.endl
	
.local gamedli	
dli	pha
	txa
	pha
	ldx #90
@	sta wsync
	mva #$1e colpf0
	mva #$26 colpf0+2
	mva #$76 colpf0+3
	;mva #$00 colpf0+4
	
	sta wsync
	mva #$14 colpf0
	mva #$6a colpf0+2
	mva #$b8 colpf0+3
	
	;mva #$02 colpf0+4
	dex
	bne @-
	pla
	tax
	pla
	rti
.endl
	
ingame_dl
	dta $70,$70,$f0
	dta $44,a(vram)
:24	dta $4
	dta $41,a(ingame_dl)
	
	
NMI	bit nmist
	bpl nmi_vbi	;vbi
	jmp (dli_ptr)	;dli
nmi_vbi	jmp (vbi_ptr)


	.align $400
gamefont	ins "alterfont.fnt"

	org $1000
vram	ins "altermap3b_7c.atrmap.dat"
	;ins "altermap2.atrmap.dat"

