#!/usr/bin/perl 

#Author: James Koval
#License: BSD see http://creativecommons.org/licenses/BSD

#Takes one argument of a file name.
#Opens the file and processes each line,
#outputting the cisco router config after each line
#The file should have "ip user pass" on each line and look like this
#10.10.10.10 JoeUser pa$$\/
#11.10.10.10 JoeUser2 pa$\/\/0Rd
#12.10.10.10 JoeUser3 pa$$\/\/0Rd

###########################################################

#use warnings; #there is a warning in the Net::Telnet::Cisco library
use strict;
use Net::Telnet::Cisco;
use subs qw(decrypt try);

my $ipRegex = "(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";#matches 0 through 255

my @xlat = ( 0x64, 0x73, 0x66, 0x64, 0x3b, 0x6b, 0x66, 0x6f, 0x41, 0x2c, 0x2e, 0x69, 0x79, 0x65, 0x77, 0x72, 0x6b, 0x6c, 0x64, 0x4a, 0x4b, 0x44, 0x48, 0x53 , 0x55, 0x42 );

my %accounts;
my @ips;
my $session;

while(<>){
  my ($ip, $user, $pass);

  #grab a valid IP followed by a user name and password, each seperated by any number of spaces
  ($ip, $user, $pass) = ($1, $2, $3) if /\b((?:$ipRegex\.){3}$ipRegex)\s+(\S+)\s+(\S+)/;
  unless( defined $ip and defined $user and defined $pass){
    push @ips, $1 if /\b((?:$ipRegex\.){3}$ipRegex)\s+/;
    next;
  }

  $session = Net::Telnet::Cisco->new(Host => $ip);
  $session->login($user, $pass);

  my @output = $session->cmd('show config');
  $session->close;

  for(@output){
    if (/username\s+(\S+)\s+.*?7\s+(\S+)/){
      $accounts{$1} = decrypt $2;
    }
  }
}

for(@ips){
  my $session = Net::Telnet::Cisco->new(Host => $_);

  while (my ($user, $pass) = each(%accounts)){
    my $ok = $session->login($user, $pass);
    if ($ok){
      my @output = $session->cmd('show config');
      $session->close;

      for(@output){
        if (/username\s+(\S+)\s+.*?7\s+(\S+)/){
          $accounts{$1} = decrypt $2;
        }
      }
    }
    else {
      sleep 1;
    }
  }
}

while (my ($key, $value) = each(%accounts)){
  print "$key  $value\n";
}

#This decrypt function borrows heaviliy from information on this site:
#http://www.tech-faq.com/decrypt-cisco-passwords.html

#The origional copyright is here:
# $Id: ios7decrypt.pl,v 1.1 1998/01/11 21:31:12 mesrik Exp $
# Credits for original code and description   hobbit@avian.org,
# SPHiXe, .mudge et al. and for John Bashinski 
# for Cisco IOS password encryption facts.
# Use of this code for any malicious or illegal purposes is strictly prohibited!

sub decrypt{
  my $hash = shift;

  my $pass = "";
  my ($s, $e);

  ($s, $e) = ($hash =~ /^(..)(.+)/o);

  for (my $i = 0; $i < length($e); $i+=2) {
    $pass .= sprintf "%c",hex(substr($e,$i,2))^$xlat[$s++];
  }

  return $pass;
}
