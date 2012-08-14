set number "line numbers
set tabstop=2 "spaces for a tab
set shiftwidth=2 "spaces for indent
set softtabstop=2 "backspace deletes spaces that filled for tab
set expandtab "use spaces rather than tabs
set smartindent "indents after {
set autoindent "keep the current indent for new lines
"set undodir=~/.vim/undodir "persistent undos between editing sessions
"set undofile "Doesn't work with vim in ubuntu 10.04, latest LTS
filetype on "detect filetype
filetype indent on "indent based on filetype
filetype plugin on "load related plugins
syntax enable "Local scope syntax highlighting
set ignorecase "ignore case searches
set incsearch "search while typing
set hlsearch  "highlight searches
set pastetoggle=<F2> "f2 for paste mode, disabling indent
nmap <F3> :set invnumber<CR>
set mouse=a "mouse compatability
set autoread "reload changed files when unedited
set linebreak "wrap lines at natural word dividers
set title "set terminal title to file name
set nowrapscan "don't wrap back to top searches
set nowrap "don't wrap long lines of text. ':set wrap' to reenable
colorscheme elflord "no dark blue, but instead use pretty syntax colors
autocmd BufWritePre * :call Preserve("%s/\\s\\+$//e") "Delete trailing whitespace on write
let @z='/{V%zfj' "fold next set of matching braces
let @x='gg:set foldmethod=manual1000@z' "fold whole file
"zM closes all folds, zR opens all, zo opens one, zc closes one

"space bar creates folds for entire file
"map <space> @x

set fillchars=fold:\ "Don't append hyphens - at the end of folds, use spaces
hi Folded ctermbg=none "Folds respect terminal transparency

"complete current word with tab, looking upwards for matches
"use <ctrl>v<tab> if you need an actual tab
inoremap <TAB> <C-P>
"r replaces visual selection with yank's buffer
vmap r "_dP

"call pathogen#infect() "autoload vim modules installed through git
"git://github.com/tpope/vim-pathogen.git
"symlink pathogen.vim to .vim/autoload
"cd .vim/bundle and git clone vim modules!
"git clone git://github.com/kchmck/vim-coffee-script.git
"git clone https://github.com/scrooloose/nerdtree.git
"git clone https://github.com/fholgado/minibufexpl.vim.git
"
"https://github.com/majutsushi/tagbar.git
"nmap <F8> :TagbarToggle<CR>
map <leader>t :TagbarToggle<CR>

map <leader>n :NERDTreeToggle<CR>

function! Preserve(command)
    let _s=@/
    let l = line(".")
    let c = col(".")
    execute a:command
    let @/=_s
    call cursor(l, c)
endfunction

"Use ctrl+h,j,k,l to move among split buffers
nmap <silent> <C-k> :wincmd k<CR>
nmap <silent> <C-j> :wincmd j<CR>
nmap <silent> <C-h> :wincmd h<CR>
nmap <silent> <C-l> :wincmd l<CR>
nmap <silent> <C-n> :vsplit <CR> "ctrl+n create new buffer

vmap <C-c> y:call system("pbcopy", getreg("\""))<CR>

" Search for selected text, forwards or backwards.
vnoremap <silent> * :<C-U>
  \let old_reg=getreg('"')<Bar>let old_regtype=getregtype('"')<CR>
  \gvy/<C-R><C-R>=substitute(
  \escape(@", '/\.*$^~['), '\_s\+', '\\_s\\+', 'g')<CR><CR>
  \gV:call setreg('"', old_reg, old_regtype)<CR>
vnoremap <silent> # :<C-U>
  \let old_reg=getreg('"')<Bar>let old_regtype=getregtype('"')<CR>
  \gvy?<C-R><C-R>=substitute(
  \escape(@", '?\.*$^~['), '\_s\+', '\\_s\\+', 'g')<CR><CR>
  \gV:call setreg('"', old_reg, old_regtype)<CR>
