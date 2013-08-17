#!/bin/bash
#Printer v1.2
#
#Options
#$1: commands like Specify printer, reset message to ip of printer, etc
#$2: IP address specified
#$3: Message specified
#
#Variables
#send:	   send a command out at the end of the script
#getip:      get ip interactively while script runs
#getmessage: get message interactively while script runs
#helpme:     print help message
#
#Defaults
#Prefer to use command line arguments over interactive approach for getting info
#The following lists are used when info isn't specified:
#    cs: for messages
#    ip: for ip addresses
#Automatically send the message to the printer
#With no commands, print help
#
###############################################
#Set default ip and message action
###############################################
ip=`fortune ip`	    #automatically use ip list
message=`fortune cs`   #automatically use cs list
send=1				 #automatically send command

if [ -z $1 ]; then	 #without arguments, show help
  helpme=1
  send=0
fi

###############################################
#Set ip and message based on Options
###############################################
if [ -n $2 ]; then
  ip=$2
fi
if [ -n $3 ]; then
  message=$3
fi
###############################################
#Set ip/message based on first argument
###############################################
if [ -n "$1" ]; then

  #CS
  if [ "$1" = "cs" ]; then
    #If no ip was passed through an option, get it interactively
    if [ -z $2 ]; then
      getip=1
    fi
  fi

  #Fortune
  if [ "$1" = "fortune" ]; then
    message=`fortune -n 40 -s`
    #If no ip was passed through an option, get it interactively
    if [ -z "$2" ]; then
      getip=1
    fi
  fi

  #Custom
  if [ "$1" = "custom" ]; then
    #If no ip/message was passed through an option, get it interactively
    if [ -z $2 ]; then
      getip=1
    fi
    if [ -z $3 ]; then
      getmessage=1
    fi
  fi

  #Reset
  if [ "$1" = "reset" ]; then
    message="${ip} Ready"
  fi

  #Help
  if [ "$1" = "help" ]; then
    helpme=1
    send=0
  fi
fi


###############################################
#Set each argument if needed
###############################################
if [ "$getmessage" = 1 ]; then
  echo -n Please enter a message:
  read message
fi
if [ "$getip" = 1 ]; then
  echo -n Please enter an IP address:
  read ip
fi


###############################################
#Send message "$message" to printer "$ip"
###############################################
if [ "$send" = 1 ]; then
  echo -n `date +%R`
  echo -n "Sending \""
  echo -n $message
  echo -n "\" to the printer "
  echo $ip
  echo @PJL RDYMSG DISPLAY=\"$message\" | netcat -q 0 $ip 9100
fi


###############################################
#Output the help info and documentation
###############################################
if [ "$helpme" = 1 ]; then
  echo "NAME"
  echo "    printer - send a message to a printer's LCD display"
  echo "              default is to send random message from ip/ip.dat and cs/cs.dat"
  echo
  echo "SYNOPSIS"
  echo "    printer [fortune | cs | custom | help | reset] [ipAddress] [Message]"
  echo
  echo DESCRIPTION
  echo "    fortune"
  echo "        Grab a random quote from fortune that fits on an LCD screen"
  echo "        and send it to a specified ip for a printer"
  echo "        You must enter an IP address along with this option"
  echo
  echo "    cs"
  echo "        Grab a random cs quote from a cs and cs.dat file pair"
  echo "        You may specify an IP address to send this message to"
  echo "        To generate a file pair, put quotes in a file"
  echo "        Seperate each line with a % on its own line"
  echo "        strfile cs cs.dat //this command generates cs.dat from cs"
  echo
  echo "    custom"
  echo "        Enter a message to display on a specified ip for a printer"
  echo
  echo "    reset"
  echo "        Sets the message to the ip of the printer"
  echo
  echo "    help"
  echo "        Display this help documentation"
fi
