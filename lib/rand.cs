// Modified by Lugnut to allow for arbitrary precision using Ipquarx's arbitrary precision arithmetic Math.cs.

function createRandContext(%seed) {
	if (%seed $= "") {
		%seed = getRealTime();
	}

	$_RNGState[$_RNGCount++] = %seed;
	$_RNGSeed[$_RNGCount] = %seed;

	return $_RNGCount;
}

function rand(%id, %min, %max, %precision) {
	%a = Math_Add(Math_Multiply("7", "47"), "1");
	%c = "100";
	%m = Math_Subtract(Math_Multiply("48", "48"), "1");

	%result = ($_RNGState[%id] = LCG($_RNGState[%id], %a, %c, %m)) / %m;

	if (%max $= "") {
		if (%min !$= "") {
			%result = Math_Multiply(%result, %min);
		}
	}
	else {
		%result = Math_Add(%min, Math_Multiply(%result, Math_Subtract(%max, %min)));
	}

	// if (%precision !$= "") { // removed and replaced to limit only to integers
	// 	%result = mFloatLength(%result, %precision); 
	// }

	// does it need a replacement?

	return %result;
}

function getRandSeed(%id) { return $_RNGSeed[%id]; }
function setRandSeed(%id, %seed) { $_RNGState[%id] = $_RNGSeed[%id] = %seed; }
function LCG(%state, %a, %c, %m) { return Math_Mod(Math_Add(Math_Multiply(%a, %state), %c), %m); }