exec("./lib/Math.cs");
exec("./src/prime.cs")

function mod(%a,%b)
{
	return Math_Subtract(%a, Math_Multiply(%b, Math_DivideFloor(%a,%b)));
}

$pubPrime = 23;
$pubBase = 5;
$aliceSecret = 5;
$bobSecret = 15;

$alicePublic = mod(math_pow($pubBase, $aliceSecret), $pubPrime);

echo("Alice Public:" SPC $alicePublic);

$bobPublic = mod(math_pow($pubBase, $bobSecret), $pubPrime);

echo("Bob Public:" SPC $bobPublic);

$secret = mod(math_pow($bobPublic, $aliceSecret), $pubPrime);

echo("Secret Num:" SPC $secret);

function Math_fastExpMod(%x, %e, %m) // this is the basic function, it needs to be implemented utilizing APA (Abritrary Precision Arithmetic).
{
	%y = 1;

	%z = x;

	while(%e > 0)
	{
		if(%e % 2 != 0)
		{
			%y = (%y * %z) % %m;
		}

		%z = (%z * %z) % %m;

		%e = mFloor(%e / 2);
	}

	return %y;
}