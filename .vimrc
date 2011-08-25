set number "line numbers
set tabstop=4 "spaces for a tab
set shiftwidth=4 "spaces for indent
set softtabstop=4 "backspace deletes spaces that filled for tab
set expandtab "use spaces rather than tabs
set smartindent "indents after {
set autoindent "keep the current indent for new lines
filetype on "detect filetype
filetype indent on "indent based on filetype
filetype plugin on "load related plugins
syntax enable "Local scope syntax highlighting
set ignorecase "ignore case searches
set incsearch "search while typing
set hlsearch  "highlight searches
set pastetoggle=<F2> "f2 for paste mode, disabling indent
set mouse=a "mouse compatability
set autoread "reload changed files when unedited
set linebreak "wrap lines at natural word dividers
set title "set terminal title to file name
set nowrapscan "don't wrap back to top searches
set nowrap "don't wrap long lines of text. ':set wrap' to reenable
colorscheme elflord "no dark blue, and pretty syntax colors
au BufWinLeave * mkview "save folds when closing vim
au BufWinEnter * silent loadview "load saved folds when reopening a file

"complete current word with tab, looking upwards for matches
"use <ctrl>v<tab> if you need an actual tab
inoremap <TAB> <C-P>
"r replaces visual selection with copy/paste buffer
vmap r "_dP

"Works only in recent vim update, not installed on most systems yet
"set undodir=~/.vim/undodir "undo between sessions
"set undofile

"not sure why I have these two or what they do exactly; comments maybe wrong
"set cindent "use C syntax for figuring tabs
"set cinkeys=0{,0},:,0#,!^F "place more #'s on new lines, after previous commented lines
"
"Trying to force mousewheel to always scroll just a few lines
"  never scroll entire pages, but doesn't work
"map <ScrollWheelUp> <C-Y>
"map <S-ScrollWheelUp> <C-Y>
"map <ScrollWheelDown> <C-E>
"map <S-ScrollWheelDown> <C-Y>
