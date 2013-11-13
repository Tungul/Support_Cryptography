// ====================== //
// McTwist's Math library //
// ====================== //
// Integer allowed only

function Math::onAdd(%this)
{
	if (!isObject(%this))
		return;
	
	%this.secure = 1;
}

// Set secure mode
function Math::secure(%secure)
{
	%this.secure = %secure;
}

// Get maximum
function Math::max(%a, %b)
{
	if (Math.secure)
	{
		%a = filterString(%a, "0123456789-");
		%b = filterString(%b, "0123456789-");
	}
	%as = strlen(%a);
	%bs = strlen(%b);
	// A is bigger
	if (%as > %bs)
		return %a;
	// B is bigger
	else if (%as < %bs)
		return %b;
	
	// Deeper comparison
	for (%i = 0; %i < %as; %i++)
	{
		%c = getSubStr(%a, %i, 1);
		%d = getSubStr(%b, %i, 1);
		// A is bigger
		if (%c > %d)
			return %a;
		else if (%c < %d)
		// B is bigger
			return %b;
	}
	// Equal
	return %a;
}

// Get minimum
function Math::min(%a, %b)
{
	if (Math.secure)
	{
		%a = filterString(%a, "0123456789-");
		%b = filterString(%b, "0123456789-");
	}
	%as = strlen(%a);
	%bs = strlen(%b);
	// B is smaller
	if (%as > %bs)
		return %b;
	// A is smaller
	else if (%as < %bs)
		return %a;
	
	// Deeper comparison
	for (%i = 0; %i < %as; %i++)
	{
		%c = getSubStr(%a, %i, 1);
		%d = getSubStr(%b, %i, 1);
		// B is smaller
		if (%c > %d)
			return %b;
		// A is smaller
		else if (%c < %d)
			return %a;
	}
	// Equal
	return %a;
}

// Remove decimals
function Math::floatLength(%num, %length)
{
	if (Math.secure)
		%num = filterString(%num, "0123456789-.");
	
	// Split number
	%float = nextToken(%num, "int", ".");
	
	// Remove extras
	%float = getSubStr(%float, 0, %length);
	
	// Add zeroes
	while (strlen(%float) < %length)
		%float = %float @ "0";
	
	// Decimals
	if (%float !$= "")
		%int = %int @ "." @ %float;
	
	return %result;
}

// Round
function Math::round(%number)
{
	
}

// Floor
function Math::floor(%number)
{
	if (Math.secure)
		%num = filterString(%num, "0123456789-.");
	
	// Split number
	%float = nextToken(%num, "int", ".");
	return %int;
}

// Ceil
function Math::ceil(%number)
{
	if (Math.secure)
		%num = filterString(%num, "0123456789-.");
	
	// Split number
	%float = nextToken(%num, "int", ".");
	if (Math::max(%float, "0") $= "0")
		%int = Math::add(%int, "1");
	return %int;
}

// Clean up dirty number
function Math::clean(%num, %point)
{
	if (Math.secure)
		%num = filterString(%num, "0123456789-.");
	// Handle negative number
	if (strpos(%num, "-") == 0)
	{
		%num = getSubStr(%num, 1, strlen(%num));
		%neg = 1;
	}
	// Remove zeroes
	while (getSubStr(%num, 0, 1) $= "0" && strlen(%num) > 1)
		%num = getSubStr(%num, 1, strlen(%num));
	
	// Handle floats
	%float = nextToken(%num, "int", ".");
	if (%point && %float !$= "")
	{
		// Remove zeroes
		while (getSubStr(%float, strlen(%float)-1, 1) $= "0")
			%float = getSubStr(%float, 0, strlen(%float)-1);
		%num = %int;
		if (%float !$= "")
			%num = %num @ "." @ %float;
	}
	else
		%num = %int;
	
	// Add negative
	if (%neg && %num !$= "0")
		%num = "-" @ %num;
	
	return %num;
}

// Addition
function Math::add(%a, %b)
{
	if (Math.secure)
	{
		%a = filterString(%a, "0123456789-");
		%b = filterString(%b, "0123456789-");
	}
	%result = "";
	%extra = 0;
	// Handle negative numbers
	if (strpos(%a, "-") == 0)
	{
		%a = getSubStr(%a, 1, strlen(%a));
		%an = 1;
	}
	if (strpos(%b, "-") == 0)
	{
		%b = getSubStr(%b, 1, strlen(%b));
		%bn = 1;
	}
	%a = "0" @ %a;
	%b = "0" @ %b;
	
	%max = getMax(strlen(%a), strlen(%b));
	// Flip them
	for (%i = 0; %i < %max; %i++)
	{
		%c = getSubStr(%a, %i, 1);
		%d = getSubStr(%b, %i, 1);
		if (%c !$= "")
			%cf = %c @ %cf;
		if (%d !$= "")
			%df = %d @ %df;
	}
	%a = %cf;
	%b = %df;
	//echo(%a SPC %b);
	%negative = 0;
	// Calculate
	for (%i = 0; %i < %max; %i++)
	{
		%c = (%an ? -1 : 1) * getSubStr(%a, %i, 1);
		%d = (%bn ? -1 : 1) * getSubStr(%b, %i, 1);
		%e = %c + %d + %extra;
		%negative = (%e < 0);
		if (%negative)
			%e += 10;
		%result = getSubStr(%e, strlen(%e)-1, 1) @ %result;
		%extra = getSubStr(%e, 0, strlen(%e)-1);
		if (%negative)
			%extra = -1;
		//echo(%c @ "+" @ %d @ "=" @ %e SPC %result SPC %extra);
	}
	
	// Remove zeroes
	while (getSubStr(%result, 0, 1) $= "0" && strlen(%result) > 1)
		%result = getSubStr(%result, 1, strlen(%result));
	
	// Handle negative number
	if (%negative)
	{
		%filler = "";
		%size = strlen(%result);
		for (%i = 0; %i < %size; %i++)
			%filler = "0" @ %filler;
		%filler = "1" @ %filler;
		%result = "-" @ Math::add(%filler, "-" @ %result);
	}
	
	return %result;
}

// Old method
function Math::add2(%a, %b)
{
	%result = "";
	%extra = 0;
	
	%max = getMax(strlen(%a), strlen(%b));
	// Flip them
	for (%i = 0; %i < %max; %i++)
	{
		%c = getSubStr(%a, %i, 1);
		%d = getSubStr(%b, %i, 1);
		if (%c !$= "")
			%cf = %c @ %cf;
		if (%d !$= "")
			%df = %d @ %df;
	}
	%a = %cf;
	%b = %df;
	// Calculate
	for (%i = 0; %i < %max; %i++)
	{
		%c = getSubStr(%a, %i, 1) << 0;
		%d = getSubStr(%b, %i, 1) << 0;
		%e = %c + %d + %extra;
		%result = getSubStr(%e, strlen(%e)-1, 1) @ %result;
		%extra = getSubStr(%e, 0, strlen(%e)-1);
		//echo(%result SPC %extra);
	}
	
	return %result;
}

// Subtraction
function Math::sub(%a, %b)
{
	if (Math.secure)
		%b = filterString(%b, "0123456789-");
	if (strpos(%b, "-") == 0)
		%b = getSubStr(%b, 1, strlen(%b));
	else
		%b = "-" @ %b;
	return Math::add(%a, %b);
}

// Multiplication
function Math::mul(%a, %b)
{
	if (Math.secure)
	{
		%a = filterString(%a, "0123456789-");
		%b = filterString(%b, "0123456789-");
	}
	%result = "";
	%extra = 0;
	
	// Handle negative numbers
	if (strpos(%a, "-") == 0)
	{
		%a = getSubStr(%a, 1, strlen(%a));
		%neg = !%neg;
	}
	if (strpos(%b, "-") == 0)
	{
		%b = getSubStr(%b, 1, strlen(%b));
		%neg = !%neg;
	}
	
	%a = "0" @ %a;
	%b = "0" @ %b;
	
	%max = getMax(strlen(%a), strlen(%b));
	// Flip them
	for (%i = 0; %i < %max; %i++)
	{
		%c = getSubStr(%a, %i, 1);
		%d = getSubStr(%b, %i, 1);
		if (%c !$= "")
			%cf = %c @ %cf;
		if (%d !$= "")
			%df = %d @ %df;
	}
	%a = %cf;
	%b = %df;
	//echo(%a SPC %b);
	// Calculate
	for (%i = 0; %i < %max*%max; %i++)
	{
		%p = %i % %max;
		%n = mFloor(%i / %max);
		// Add leading zeroes
		if (%p $= 0)
		{
			for (%o = 0; %o < %n; %o++)
				%result[%n] = "0" @ %result[%n];
		}
		
		%c = getSubStr(%a, %p, 1) * 1;
		%d = getSubStr(%b, %n, 1) * 1;
		%e = (%c * %d) + %extra;
		%result[%n] = getSubStr(%e, strlen(%e)-1, 1) @ %result[%n];
		%extra = getSubStr(%e, 0, strlen(%e)-1);
		//echo(%c @ "+" @ %d @ "=" @ %e SPC %result[%n] SPC %extra);
	}
	
	//while (getSubStr(%result, 0, 1) $= "0")
	//	%result = getSubStr(%result, 1, strlen(%result));
	// Remove zeroes
	while (getSubStr(%result, 0, 1) $= "0" && strlen(%result) > 1)
		%result = getSubStr(%result, 1, strlen(%result));
	
	// Add together
	%result = %result[0];
	for (%i = 1; %i < %max; %i++)
		%result = Math::add(%result, %result[%i]);
	
	// Negative number
	if (%neg)
		%result = "-" @ %result;
	
	return %result;
}

// Division
function Math::div(%a, %b)
{
	if (Math.secure)
	{
		%a = filterString(%a, "0123456789-");
		%b = filterString(%b, "0123456789-");
	}
	%result = "";
	%rest = 0;
	
	// Handle negative numbers
	if (strpos(%a, "-") == 0)
	{
		%a = getSubStr(%a, 1, strlen(%a));
		%neg = !%neg;
	}
	if (strpos(%b, "-") == 0)
	{
		%b = getSubStr(%b, 1, strlen(%b));
		%neg = !%neg;
	}
	
	// Lower than 1
	if (Math::max(%a, %b) $= %b)
		return "1";
	
	%max = strlen(%a);
	
	// Divide
	%c = "";
	for (%i = 0; %i < %max; %i++)
	{
		%p = 0;
		
		%c = %c @ getSubStr(%a, %i, 1);
		%p = mFloor(%c / %b);
		//echo(%c SPC %p);
		// Too smal division
		if (%p < 1)
		{
			%result = %result @ "0";
			continue;
		}
		
		// Get rest
		%c = %c % %b;
		//echo(%c SPC %p);
		%result = %result @ %p;
	}
	
	// Remove leading zeroes
	while (getSubStr(%result, 0, 1) $= "0" && strlen(%result) > 1)
		%result = getSubStr(%result, 1, strlen(%result));
	
	// Negative number
	if (%neg)
		%result = "-" @ %result;
	
	return %result;
}

// Modulus
function Math::mod(%number, %mod)
{
	if (Math.secure)
	{
		%number = filterString(%number, "0123456789-");
		%mod = filterString(%mod, "0123456789-");
	}
	%take = 5;
	%result = "";
	
	// Calculate modulus
	%size = strlen(%number);
	for (%i = 0; %i < %size; %i += %take)
	{
		%a = %result @ getSubStr(%number, %i, %take);
		%result = %a % %mod;
	}
	
	return %result;
}

// Power of
function Math::pow(%number, %pow)
{
	if (Math.secure)
	{
		%number = filterString(%number, "0123456789-");
		%pow = filterString(%pow, "0123456789-");
	}
	if (%pow $= "0" || getSubStr(%pow, 0, 1) $= "-")
		return "1";
	// Do-while, Old-school power with multiplication
	%result = %number;
	%pow = Math::sub(%pow, "1");
	while (%pow !$= "0")
	{
		%result = Math::mul(%result, %number);
		%pow = Math::sub(%pow, "1");
	}
	return %result;
}

// Shift a number left or right
function Math::shift(%num, %shift)
{
	if (Math.secure)
	{
		%num = filterString(%num, "0123456789-");
		%shift = filterString(%shift, "0123456789-");
	}
	// No changes
	if (%shift $= "0")
		return %num;
	// Slow, but works
	if (getSubStr(%shfit, 0, 1) $= "-")
		return Math::div(%num, Math::pow("2", Math::sub(%shift, "1")));
	else
		return Math::mul(%num, Math::pow("2", Math::sub(%shift, "1")));
}

// Base converstion
function Math::base(%number, %base)
{
	// Invalid base
	if (%base <= 1)
		return %number;
	// Character base
	if (%base <= 64)
	{
		%string = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_";
	}
	// Binary base
	else
	{
		//warn("Math::base - Base too large");
		//return %number;
		for (%i = 0; %i < 256; %i++)
			%string = %string @ chr(%i);
	}
	%base = getSubStr(%string, 0, %base);
	if (Math.secure)
		%number = filterString(%number, %base);
	return Math::base2dec(%number, %base);
}

// Decimal conversion
function Math::dec(%number, %base)
{
	// Invalid base
	if (%base <= 1)
		return %number;
	// Character base
	if (%base <= 64)
	{
		%string = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_";
	}
	// Binary base
	else
	{
		//warn("Math::base - Base too large");
		//return %number;
		for (%i = 0; %i < 256; %i++)
			%string = %string @ chr(%i);
	}
	if (Math.secure)
		%number = filterString(%number, "0123456789");
	return Math::dec2base(%number, getSubStr(%string, 0, %base));
}

// Decimal to base conversion
function Math::dec2base(%num, %digits)
{
	if (Math.secure)
		%num = filterString(%num, "0123456789");
	
	%size = strlen(%num);
	%base = strlen(%digits);
	%result = "";
	
	// Calculate character through number
	while (%num > %base-1)
	{
		%rest = Math::mod(%num, %base);
		%num = Math::div(%num, %base);
		%result = getSubStr(%digits, %rest, 1) @ %result;
	}
	%result = getSubStr(%digits, %num, 1) @ %result;
	return %result;
}

// Base to decimal conversion
function Math::base2dec(%num, %digits)
{
	if (Math.secure)
		%num = filterString(%num, %digits);
	
	%size = strlen(%num);
	%base = strlen(%digits);
	%result = "0";
	
	// Calculate number through character
	for (%i = 0; %i < %size; %i++)
	{
		%element = strpos(%digits, getSubStr(%num, %i, 1));
		%power = Math::pow(%base, %size-%i-1);
		%result = Math::add(%result, Math::mul(%element, %power));
	}
	
	return %result;
}

// Create namespace object
if (!isObject(Math))
{
	new ScriptObject(Math);
}

// Convert number to character
function chr(%n)
{
	%n = Math::dec2base(%n, "0123456789ABCDEF");
	if (strlen(%n) < 2)
		%n = "0" @ %n;
	return eval("%z = \"\\x" @ %n @ "\";");
}