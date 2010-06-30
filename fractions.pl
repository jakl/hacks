use strict;
use warnings;
use subs qw(gcd lcm reduce commondenom);

my @a;
my @b;

$a[0] = 256;
$a[1] = 512;#1/2

$b[0] = 6;#3/4
$b[1] = 32;

print "Format: 5/7 4/3 = 5 7 4 3\n";

print "Initialized: @a @b \nEnd\n";

reduce ([@a], [@b]);# [var] is pass by reference

print "Reduced: @a @b \nEnd\n";

commondenom ([@a], [@b]);

print "Common Denom'd: @a @b \nEnd\n";

print "Added: ".($a[0]+$b[0])." $a[1]";

sub gcd{
	my $a = shift;
	my $b = shift;
	#print "$a\n$b\n";
	return $b if($a == 0);
	gcd($b%$a,$a);
}

sub lcm{
	my $a = shift;
	my $b = shift;
	return $a*$b/gcd($a,$b);
}

sub reduce{
	my ($a,$b) = @_;# $var = @_ catches a reference
	my $div = gcd ($a[0], $a[1]);
	$_/=$div for @a;# for @var cycles references to the array values
	$div = gcd ($b[0], $b[1]);
	$_/=$div for @b;
}

sub commondenom{
	my ($a,$b) = @_;
	my $denom = lcm($a[1],$b[1]);
	$a[0] *= $denom/$a[1];
	$b[0] *= $denom/$b[1];
	$a[1] = $b[1] = $denom;
}