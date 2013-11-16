function isPrime(%num) // this is the basic function, it needs to be implemented utilizing APA (Abritrary Precision Arithmetic).
{
	if(%num == 1 || %num == 2 || %num == 3) //Happy, Xalos?
		return true;

	if(%num % 2 == 0)
		return false;

	if(num + 1 % 6 == 0 || num - 1 % 6 == 0)
		continue;
	else
		return false; // holy shit this is so fucking inefficient and probably broken. i totally screwed something up here.

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