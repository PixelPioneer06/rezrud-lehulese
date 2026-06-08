2000 constant MAX-NODES
create Told MAX-NODES floats allot
create Tnew MAX-NODES floats allot

variable NODES
variable SUB-STEPS       
variable DRAW-ASCII?     
1 DRAW-ASCII? !          

fvariable ALPHA
fvariable BETA
fvariable TENV
fvariable STEPT          
fvariable DX             

fvariable TOTAL-TIME     
fvariable OUT-STEP       

1.16e-4 ALPHA f!
1.74e-3 BETA  f!
2.0e1   TENV  f!

fvariable t-i
fvariable t-prev
fvariable t-next
fvariable dT-diff
fvariable dT-conv

: init-variables
  NODES @ 0= if 61 NODES ! then
  TOTAL-TIME f@ 0.0e0 f= if 10.0e0 TOTAL-TIME f! then
  OUT-STEP f@ 0.0e0 f= if 0.5e0 OUT-STEP f! then ;

: valid-nodes? ( -- flag )
  NODES @ 3 >= NODES @ MAX-NODES <= and ;


: auto-tune-physics
  
  1.0e0 NODES @ 1- s>f f/ DX f!
  
  
  DX f@ DX f@ f* 4.0e0 ALPHA f@ f* f/ 
  
  
  fdup OUT-STEP f@ fswap f/ f>s 1+ SUB-STEPS !
  
  
  OUT-STEP f@ SUB-STEPS @ s>f f/ STEPT f! 
  fdrop ;

: init-rod
  NODES @ 0 do
    TENV f@ i floats Told + f!
    TENV f@ i floats Tnew + f!
  loop
  
  5.0e2 NODES @ 2 / floats Told + f!
  5.0e2 NODES @ 2 / floats Tnew + f! ;

: boundary-conditions
  1 floats Told + f@  0 floats Told + f!
  NODES @ 2 - floats Told + f@  NODES @ 1- floats Told + f!
  1 floats Tnew + f@  0 floats Tnew + f!
  NODES @ 2 - floats Tnew + f@  NODES @ 1- floats Tnew + f! ;

: update-nodes
  NODES @ 1- 1 do
    i     floats Told + f@  t-i    f!
    i 1 - floats Told + f@  t-prev f!
    i 1 + floats Told + f@  t-next f!
    
    t-next f@ t-i f@ f- t-i f@ f- t-prev f@ f+
    ALPHA f@ f* DX f@ DX f@ f* f/
    dT-diff f!
    
    t-i f@ TENV f@ f- BETA f@ f*
    dT-conv f!
    
    t-i f@ dT-diff f@ dT-conv f@ f- STEPT f@ f* f+
    i floats Tnew + f!
  loop ;

: copy-new-to-old
  NODES @ 0 do
    i floats Tnew + f@  i floats Told + f!
  loop ;

: print-rod
  NODES @ 0 do
    i .  i floats Told + f@ f. cr
  loop ;

: draw-rod-ascii ( f: time -- f: time )
  cr ." === IDO: " fdup f. ." s ===" cr
  0 35 do
    NODES @ 0 do
      i floats Told + f@ 15.0e0 f/ f>s
      j >= if 
        ." #" 
      else 
        ."  " 
      then
    2 +loop 
    cr 
  -1 +loop 
  
  NODES @ 0 do ." -" 2 +loop cr ;

: simulate
  init-variables
  valid-nodes? 0= if cr ." Hiba: Ervenytelen rácsfelbontás!" cr exit then
  auto-tune-physics
  init-rod
  
  0.0e0 
  begin
    fdup TOTAL-TIME f@ f<=
  while
    
    DRAW-ASCII? @ 1 = if
      draw-rod-ascii
    else
      cr ." Ido: " fdup f. cr
      boundary-conditions
      print-rod
      cr cr
    then

    
    SUB-STEPS @ 0 do
      boundary-conditions
      update-nodes
      copy-new-to-old
    loop
    
    
    OUT-STEP f@ f+
  repeat
  fdrop ;
