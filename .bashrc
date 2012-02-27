function load_dir {
    LOAD_DIR=${1}
    if [ -d $LOAD_DIR -a -r $LOAD_DIR -a -x $LOAD_DIR ]; then
        local i
        for i in $(find -L $LOAD_DIR -name '*.sh'); do
            source $i
        done
    fi
}

load_dir ${HOME}/.bashrc.d
