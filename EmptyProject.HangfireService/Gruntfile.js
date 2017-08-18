
module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            SSMS: {
                files: [
						{expand: false, src: ['*.dll'], dest: 'C:/Publish/EmptyProject.HangfireService/lib/', filter: 'isFile'},
						{expand: false, src: ['Config/*.*'], dest: 'C:/Publish/EmptyProject.HangfireService/', filter: 'isFile'},
						{
						    'C:/Publish/EmptyProject.HangfireService/Autofac.json': 'Autofac.json',
						    'C:/Publish/EmptyProject.HangfireService/log4net.config': 'log4net.config',
						    'C:/Publish/EmptyProject.HangfireService/EmptyProject.HangfireService.exe': 'EmptyProject.HangfireService.exe',
						    'C:/Publish/EmptyProject.HangfireService/EmptyProject.HangfireService.exe.config': 'EmptyProject.HangfireService.exe.config'
						}
					]
            }
        }
    });
    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.registerTask('default', ['copy']);
};