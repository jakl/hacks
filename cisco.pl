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

#You can even put in comments after the ip user pass like this
#10.10.10.10 joeuser pass Some comment out here
#Or have the ip come after the user pass, like "user pass ip"
#jeouser pas$ 10.10.10.10  some comment


#use warnings; #there is a warning in the Net::Telnet::Cisco library
use strict;
use Net::Telnet::Cisco;

while(<>){
  my $ipRegex = "(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";#matches 0 through 255
  my $ip = $1 if /\b((?:$ipRegex\.){3}$ipRegex)\b/;
  next unless defined $ip;
  s/$ip//;#delete the ip

  my ($user, $pass);
  if(/\b(.+?)\s+(.+?)\b/){
    $user = $1;
    $pass = $2;
  }
  next unless defined $user and defined $pass;

  my $session = Net::Telnet::Cisco->new(Host => $ip);
  $session->login($user, $pass);
  
  my @output = $session->cmd('show config');
  $session->close;

  print "$ip \n @output \n";
}
