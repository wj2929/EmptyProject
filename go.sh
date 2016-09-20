#! bin/bash
#自动进行项目初始操作的自动化脚本
#Author:@wj2929
#定义项目变量

defaultoriginalname="EmptyProject"
originalname=$defaultoriginalname
replacename=$1
if [ "$replacename" == "" ]
then
	read -p "请输入原始项目名称（默认："$defaultoriginalname"):" originalname
	[ "$originalname" == "" ] && originalname=$defaultoriginalname
	read -p "请设置替换项目名称:" replacename
	[ "$replacename" == "" ] && echo "替换项目名称不能为空！" && exit 1
fi

replace_files(){  
	for file2 in `ls -a $1`  
	do  
		if [ x"$file2" != x"." -a x"$file2" != x".." -a x"$file2" != x".git" -a x"$file2" != x".gitignore" -a x"$file2" != x"lib" -a x"$file2" != x"bin" -a x"$file2" != x"obj" -a x"$file2" != x"packages" ];then  
			if [ -d "$1/$file2" ];then  
			    replace_files $1/$file2
			elif [ -s "$1/$file2" ];then  
				if contains $file2 $originalname || contains $file2 ".sln" || contains $file2 ".csproj" || contains $file2 ".tt" || contains $file2 ".cs"
				then  
					#替换内容
					sed -i 's/'$originalname'/'$replacename'/g' $1/$file2
					#重命名文件名
					newfilepath=$1/$(echo $file2 | sed -r 's/'$originalname'/'$replacename'/g')
					[ "$1/$file2" != "$newfilepath" ] && mv $1/$file2 $newfilepath
				fi
			fi  
		fi  
	done  
}  

replace_dirs(){  
	for file2 in `ls -a $1`  
	do  
		if [ x"$file2" != x"." -a x"$file2" != x".." -a x"$file2" != x.git" -a x"$file2" != x".gitignore" -a x"$file2" != x"lib" -a x"$file2" != x"bin" -a x"$file2" != x"obj" -a x"$file2" != x"packages" ];then
			if [ -d "$1/$file2" ];then  
				replace_dirs $1/$file2
				# echo $1
				if contains $1 $originalname
				then
					# echo '3'
					#重命名文件夹名
					newdirpath=$(echo $1 | sed -r 's/'$originalname'/'$replacename'/g')
					mv $1 $newdirpath
				fi    				
			fi
		fi  
	done  
}

contains() {
    string="$1"
    substring="$2"
    if test "${string#*$substring}" != "$string"
    then
        return 0    # $substring is in $string
    else
        return 1    # $substring is not in $string
    fi
}

# echo "replace_files"
replace_files ./
# echo "replace_dirs"
replace_dirs  ./
echo "ok!"
