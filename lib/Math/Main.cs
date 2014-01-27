// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Main (Exectute this!)	//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		WHAT YOU ARE DOING		---	//
// ------------------------------------	//

// Prepare global variables used in multiplication
for(%tmp = 0; %tmp < 10000; %tmp++)
{
	%tmp2 = 4 - strlen(%tmp);
	if(%tmp2)
		%tmp = getSubStr("000", 0, %tmp2) @ %tmp;

	$NORMAL[%tmp | 0] = %tmp;
}

//Load basic integer functions
exec("./Helpers.cs");
exec("./BasicFunctions.cs");

//Load all other modules in the folder
for(%file = findFirstFile("./*.cs"); %file !$= ""; %file = findNextFile("./*.cs"))
	if((%base = fileBase(%file)) !$= "Main" && %base !$= "Helpers" && %base !$= "BasicFunctions")
		exec(%file);