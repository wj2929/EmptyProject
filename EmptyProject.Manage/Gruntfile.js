
module.exports = function (grunt) {
    'use strict';
    require('load-grunt-tasks')(grunt);
    var gruntConfig = grunt.file.readJSON('Gruntconfig.json');
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        cvars: gruntConfig.configVars,
        requirejs: {
            js: {
                options: {
                    uglify2: {
                        mangle: false
                    },
                    name: "APP",
                    baseUrl: "APP/",
                    mainConfigFile: "APP/main.js",
                    out: "APP/requirejs_main.js",
                    optimize: "uglify2",
                    preserveLicenseComments: false,
                    generateSourceMaps: true,
                    useStrict: true,
                    almond: true,
                    wrap: true,
                    insertRequire: ['main.js?t=1.5'], // <-- added this
                    findNestedDependencies: true
                }
            }
        },
        bower: {
            setup: {
                options: { install: true, copy: false }
            }
        },
        cssmin: {
            compress: {
                files: {
                    'css/compresslayout.min.css': [
                      "css/bootstrap.min.css",
                      "css/bootstrap-reset.css",
                      "css/font-awesome.min.css",
//                      "css/iconfont.css",
                      "css/style.css",
                      "css/style-responsive.css",
                      "/js/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css",
                      "css/component.css",
                      "css/custom.css",
                      "js/bootstrap-datepicker/css/datepicker.css"
                    ]
                }
            }
        },
        copy: {
            setup: {
                files: [
                // Javascript with standard .min.js naming convention
                    {
                    cwd: 'bower_components',
                    expand: true,
                    flatten: true,
                    dest: '<%= cvars.app %>/<%= cvars.appjs %>/',
                    src: gruntConfig.bowerFiles
                    }
                ]
            },
            EmptyProject: {
                files: [
                    {
                        expand: false, src: ['bin/*.dll'], dest: '../Publish/EmptyProject/', filter: 'isFile'
                    },
                    {
                    expand: false, src: ['Config/**'], dest: '../Publish/EmptyProject/'
                },
                    {
                        expand: false, src: ['fonts/**'], dest: '../Publish/EmptyProject/'
                    },
                    {
                        expand: false, src: ['img/**'], dest: '../Publish/EmptyProject/'
                    },
                    {
                        expand: false, src: ['images/**'], dest: '../Publish/EmptyProject/'
                    },
                    {
                        expand: false, src: ['font/**'], dest: '../Publish/EmptyProject/'
                    },
                    {
                        expand: false, src: ['js/**'], dest: '../Publish/EmptyProject/'
                    },
                    {
                        '../Publish/EmptyProject/css/compresslayout.min.css': 'css/compresslayout.min.css'
                    },
                    {
                        '../Publish/EmptyProject/Scripts/jquery.metadata.js': 'Scripts/jquery.metadata.js',
                        '../Publish/EmptyProject/Scripts/jquery.validate.min.js': 'Scripts/jquery.validate.min.js',
                        '../Publish/EmptyProject/Scripts/jquery.validate.unobtrusive.min.js': 'Scripts/jquery.validate.unobtrusive.min.js',
                        '../Publish/EmptyProject/Scripts/jquery.unobtrusive-ajax.min.js': 'Scripts/jquery.unobtrusive-ajax.min.js'
                    },
                    {
                        '../Publish/EmptyProject/Unity.config': 'Unity.config'
                    },
                    {
                        '../Publish/EmptyProject/Global.asax': 'Global.asax'
                    },
                    {
                        expand: false, src: ['APP/**'], dest: '../Publish/EmptyProject/'
                    },
                ]
            }
        },
        clean: {
            build: {
                src: ["app/requirejs_main.js", "app/requirejs_main.js.map", "css/compresslayout.min.css", "css/compressmanage.min.css"]
            }
        },
        targethtml: {
            Index: {
                src: 'APP/index.htm',
                dest: '../Publish/EmptyProject/APP/index.htm'
            },
            ManageIndex: {
                src: 'APP/manage/_index.html',
                dest: '../Publish/EmptyProject/APP/manage/_index.html'
            }
        },
        watch: {
            www: {
                files: ['<%= cvars.app %>/**/*'],
                tasks: [],
                options: {
                    spawn: false,
                    livereload: true
                }
            }
        },
        connect: {
            server: {
                livereload: true,
                options: {
                    port: gruntConfig.configVars.port,
                    base: '<%= cvars.app %>'
                }
            }
        },
        htmlmin: {
            // See https://github.com/yeoman/grunt-usemin/issues/44 for using 2 passes
            build: {
                options: {
                    removeComments: true,
                    collapseWhitespace: true
                },
                files: [
                    { '../Publish/EmptyProject/APP/index.htm': '../Publish/EmptyProject/APP/index.htm' },
                    {expand: true, cwd: '../Publish/EmptyProject/APP/manage', src: ['**/*.html','*.html'], dest: '../Publish/EmptyProject/APP/manage'}
                ]
            }
        },
        uglify: {
            "my_target": {
                "files": [
                    { expand: true, cwd: '../Publish/EmptyProject/APP/manage', src: ['**/*.js'], dest: '../Publish/EmptyProject/APP/manage' },
                    {
                         '../Publish/EmptyProject/APP/Common/Libs/require.min.js':'../Publish/EmptyProject/APP/Common/Libs/require.js'
                    },
                ]
            }
        }
    });

    //grunt.registerTask('setup', ['bower:setup', 'copy:setup']);
    grunt.registerTask('setup', ['copy:setup']);

    //grunt.registerTask('build', ["requirejs", 'concat', 'uglify', "cssmin", 'copy', "targethtml", "clean"]);
    grunt.registerTask('build', ["requirejs", "cssmin", 'copy:EmptyProject', "targethtml", "htmlmin:build","uglify", "clean"]);

    grunt.registerTask('devel', [
        'connect:server', 'watch:www'
      ]);
};