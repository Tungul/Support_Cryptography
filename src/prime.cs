function Math_isPrime(%num) // this is the basic function, it needs to be implemented utilizing APA (Abritrary Precision Arithmetic).
{
	if(%num $= "1" || %num $= "2" || %num $= "3") //Happy, Xalos?
		return true;

	if(Math_Mod(%num, 2) $= "0")
		return false;

	if(Math_Mod(Math_Add(%num, 1), 6) $= "0" || Math_Mod(Math_Subtact(num, 1), 6) $= "0")
		continue;
	else
		return false; // holy shit this is so fucking inefficient and probably broken. i totally screwed something up here.

	%squareRoot = Math_SquareRoot(%num); // oops we don't have one of these...

	for(%i = "3"; Math_LessThan(%i, %squareRoot); %i = Math_Add(%i, 2)) // this can be sped up if we generate a set of primes using the sieve of eratsones, or just flat out include a list of primes from like, one to a million.
	{
		if(Math_Mod(%num, %i) $= "0")
		{
			return false;
		}
	}

	return true;
}

// TODO: "Math_SquareRoot()" and "Math_LessThan()" or equivalent