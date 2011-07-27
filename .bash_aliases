alias search='apt-cache search'
alias show='apt-cache show'
alias instal='sudo apt-get install'
alias remove='sudo apt-get autoremove'
alias update='sudo apt-get update'
alias upgrade='sudo apt-get upgrade'

alias ls='ls -F --color=auto'
alias la="ls -a | grep '^\.'"
alias ff='find . -type f -iname'
alias acp='ack -i --perl' #instal ack-grep
alias when='~/bin/when.sh' #history | grep -i $1 | grep -v when
alias what='ps -e | grep -i'
alias v='vi'
alias s='screen -DR'
alias s='screen -DR'

if [ -f ~/.genome_alias ]; then
    . ~/.genome_alias
fi
