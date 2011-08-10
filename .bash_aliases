alias search='aptitude search'
alias show='aptitude show'
alias instal='sudo aptitude install'
alias remove='sudo aptitude remove'
alias update='sudo aptitude update'
alias upgrade='sudo aptitude safe-upgrade'

alias ls='ls -F --color=auto'
alias la="ls -a | grep '^\.'"
alias ff='find . -type f -iname'
#alias ack='ack-grep' #ubuntu package default is ack-grep
alias acp='ack -i --perl'
alias what='ps -e | grep -i'
alias s='screen -DR'

alias ..='cd ..'
alias ...='cd ../..'
alias ....='cd ../../..'
alias .....='cd ../../../..'
alias ......='cd ../../../../..'

when() { 
    history | grep -i $1 | grep -v when 
}

if [ -f ~/.genome_alias ]; then
    . ~/.genome_alias
fi
