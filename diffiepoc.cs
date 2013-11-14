exec("./lib/Math.cs");

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

function isPrime(%num) // this is the basic function, it needs to be implemented utilizing APA (Abritrary Precision Arithmetic).
{
	if(%num % 2 == 0)
		return false;

	if(num + 1 % 6 == 0 || num - 1 % 6 == 0)
	{
		continue;
	}
	else
	{
		return false; // holy shit this is so fucking inefficient and probably broken. i totally screwed something up here.
	}

	%squareRoot = mSqrt(%num);

	for(%i = 3; %i < %squareRoot; %i += 2) // this can be sped up if we generate a set of primes using the sieve of eratsones, or just flat out include a list of primes from like, one to a million.
	{
		if(%num % %i == 0)
		{
			return false;
		}
	}

	return true;
}

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