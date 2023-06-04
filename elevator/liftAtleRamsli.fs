


0 value currentFloor
8 value numfloors
0 value direction \ 0 is at rest, 1 is going up -1 is going down

create upButtons 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,
create downButtons 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,
create floorButtons 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,

: goingUp? ( -- f ) direction 1 = ;

: goingDown? ( -- f ) direction -1 = ;
	
\ Retrieve the upButton for floor No
: upButton ( floorNo -- addr )
    cells upButtons + ;

: pressUpButton ( floorno -- )
    upButton -1 swap ! ;
    
: clearUpButton ( floorno -- )
    upButton 0 swap ! ;

\ Print the UP button on floor floor no
: .upButton ( floorNo -- )
	upButton @ if ." [ UP ] " else ." [    ] " then ;
	
: downButton ( floorNo -- addr )
    cells downButtons + ;
   
: pressDownButton ( floorno -- )
    downButton -1 swap ! ;

: clearDownButton ( floorno -- )
    downButton 0 swap ! ;

\ retrieve the DOWN button for floor floorNo
: .downButton ( floorNo -- )
	downButton @ if ." [DOWN] " else ." [    ] " then ;

	
: floorButton ( floorNo -- addr )
    cells floorButtons + ;
    
: pressFloorButton ( floorno -- )
    floorButton -1 swap ! ;

: clearFloorButton ( floorno -- )
    floorButton 0 swap ! ;

: .floorButton ( btnno -- )
	floorButton @ if ." [*] " else ." [ ] " then ;
	

\ any buttons above us pressed ?
: buttonsAbove? ( -- f )
	0	\ assume false
	numfloors currentFloor do
		i upButton @ if
			drop -1 leave \ replace false with true and leave
		then
		i floorButton @ if
			drop -1 leave \ replace false with true and leave
		then
		i downButton @ if
			drop -1 leave \ replace false with true and leave
		then
	loop
;

\ any buttons below us pressed?
: buttonsBelow? ( -- f ) 
	currentFloor 0 = if 
		0
		exit
	then
	0	\ assume false
	currentFloor 0 do
		i upButton @ if
			drop -1 leave \ replace false with true and leave
		then
		
		i floorButton @ if
			drop -1 leave \ replace false with true and leave
		then
		
		i downButton @ if
			drop -1 leave \ replace false with true and leave
		then
	loop
;

\ We must go up if there are buttons pressed above us, 
\ unless we're going down and there are buttons pressed below
: mustGoUp? 
	currentFloor 0 < if
		-1
	else goingDown? if
		buttonsBelow?
	else
		buttonsAbove?
	then
	then
	;

: mustGoDown? 
	currentFloor numfloors = if
		-1
	else goingUp? if
		buttonsAbove?
	else
		buttonsBelow?
	then
	then
	;



\ Move lift one floor up. It will only clear UP buttons. DOWN buttons will still be lit.
\ Clearing a button means the elevator stopped and opened the doors.
\ ( I should probably have made that more explicit )
\ The DOWN buttons will be cleared on the way down.
: goUp currentFloor 1 + to currentFloor 
	currentFloor clearUpButton
	currentfloor clearFloorButton
	currentFloor numFloors > if
		numFloors to currentFloor 
	then ;
	
\ Move lift one floor down. Inverse logic of goUP.
: goDown currentFloor 1 - to currentFloor 
	currentFloor clearDownButton
	currentfloor clearFloorButton
	currentFloor 0 < if
		0 to currentFloor 
	then ;
	
\ Print the elevator panel on floor floorNo
: .floor ( floorno -- )
	cr dup .
	dup currentFloor = if
       ." *"
    else
       ." _" 
   then
	dup .floorButton ." ### "
	dup .upbutton 
    .downbutton 
    
;

\ Print the whole lift
: .lift
	cr ." #########################" cr
	." LIFT   ###  FLOOR #######" 
   numFloors 1 + 1 do
   numfloors i - .floor
   loop
   cr
   ;
   
   
: goOneFloor (  -- )
	mustGoUp? if
		cr ." Going UP"
		goUp
	else mustGoDown? if
			cr ." Going DOWN"
			goDown
		else
		cr ." Waiting"
			0 to direction
		then
	then
	;
	
: run ( noOfFloorIterations -- )   
	0 do
		goOneFloor .lift
	loop
	;

\ Usage
." Testing some low level words: "

." Inside the elevator, press buttons [4] [6] [7]" cr
4 6 7 pressFloorButton pressFloorButton pressFloorButton

." On the 3rd and 5th floor, press the [UP] button" cr
3 5 pressUpButton pressUpButton

." On the Forth, 5th and 6th floor, press the [DOWN] button" cr
4 5 7 pressUpButton pressDownButton

." The elevator is at the ground floor "
0 to currentfloor

.lift

." Send the lift up 3 floors, clears the [UP] button on floor 3" cr
goUp goUp goUp 
.lift

." Send the lift up 1 floors, clears the button [4] in the elevator" cr
goUp 
.lift

." Send the lift up 1 floors only moves the elevator" cr
goUp 
.lift

." Send the lift up 1 floors, clears button [6]" cr
goUp 
.lift

." Send the lift up 1 floors, clears button [7]" cr
goUp 
.lift

." We're at rest at the ground floor"
0 to direction
0 to currentFloor
.lift

." Press all those buuttons again and run for 10 iterations"
." Inside the elevator, press buttons [4] [6] [7]" cr
4 6 7 pressFloorButton pressFloorButton pressFloorButton

." On the 3rd and 5th floor, press the [UP] button" cr
3 5 pressUpButton pressUpButton

." On the Forth, 5th and 6th floor, press the [DOWN] button" cr
4 5 7 pressUpButton pressDownButton


10 run
