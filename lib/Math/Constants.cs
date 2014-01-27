// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Constant Methods			//
// --- This file contains methods that	//
// --- give speed improvements based on	//
// --- precomputed values, and gives	//
// --- the methods to compute them.		//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		 WHAT YOU'RE DOING		---	//
// ------------------------------------	//

$Math::Constants::Count = 0;
$Math::Constants::nextConstant = 0;

function CMath_GenerateConstant(%Number)
{
	%Ind = getword($Math::Constants::nextConstant, 0);
	$Math::Constants::nextConstant = removeWord($Math::Constants::nextConstant, 0);

	$Constants_[%Ind, 0] = 0;
	$Constants_[%Ind, 1] = %Number;

	for(%a = 2; %a < 10000; %a++)
		$Constants_[%Ind, %a] = IMath_Add($Constants_[%Ind, %a - 1], %Number);

	$Math::Constants::Count++;

	if($Math::Constants::nextConstant $= "")
		$Math::Constants::nextConstant = $Math::Constants::Count;

	return %Ind;
}

function CMath_RemoveConstant(%Constant)
{
	if($Constants_[%Constant, 1] $= "")
		return;

	deleteVariables("$Constant_" @ %Constant @ "*");

	$Math::Constants::Count--;

	//Note: The next constant variable is basically a primitive stack.
	if($Math::Constants::nextConstant !$= "")
		$Math::Constants::nextConstant = %Constant SPC $Math::Constants::nextConstant;
	else
		$Math::Constants::nextConstant = %Constant;
}

function CMath_Mod(%num, %constant)
{
	%other = $Constants_[%constant, 1];
	%take = strlen(%other) + 4;
	%r2 = getsubstr(%other, 0, 6);
	%otherl = %take - 4;

	while(alessthanb(%other, %num))
	{
		//Get a number to do a modulus on using our multiplication cache
		%tmp = getsubstr(%num, 0, %take);
		%tmpl = strlen(%tmp);

		%r1 = getsubstr(%tmp, 0, 6);

		//Calculate tmp/num2.
		%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("000000", 0, %tmpl - %otherl) | 0));

		//Make sure we're within range of the multiplication cache.
		%xx = 0;
		while(%ratio > 9999)
		{
			%tmp = getsubstr(%tmp, 0, %take-(%xx++));
			%tmpl--;
			%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("000000", 0, %tmpl - %otherl) | 0));
		}

		//Calculate the modulus and prefix it to the original number.
		//This way, we're only doing 1 subtraction every 3/4 digits of the number. This makes it very fast.
		%num = IMath_Subtract(%tmp, $Constants_[%constant, %ratio]) @ getsubstr(%num, %take - %xx, %take);
	}

	return %num;
}

function CMath_DivFloor(%num, %constant)
{
	%other = $Constants_[%constant, 1];
	%take = strlen(%other) + 4;
	%r2 = getsubstr(%other, 0, 6);
	%otherl = %take - 4;

	while(alessthanb(%other, %num))
	{
		//Get a number to do a modulus on using our multiplication cache
		%tmp = getsubstr(%num, 0, %take);
		%tmpl = strlen(%tmp);

		%r1 = getsubstr(%tmp, 0, 6);

		//Calculate tmp/num2.
		%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("000000", 0, %tmpl - %otherl) | 0));

		//Make sure we're within range of the multiplication cache.
		%xx = 0;
		while(%ratio > 9999)
		{
			%tmp = getsubstr(%tmp, 0, %take-(%xx++));
			%tmpl--;
			%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("000000", 0, %tmpl - %otherl) | 0));
		}

		%Result = %Result @ %ratio;

		//Calculate the modulus and prefix it to the original number.
		//This way, we're only doing 1 subtraction every 3/4 digits of the number. This makes it very fast.
		%num = IMath_Subtract(%tmp, $Constants_[%constant, %ratio]) @ getsubstr(%num, %take - %xx, %take);
	}

	return %Result $= "" ? "0" : %Result;
}