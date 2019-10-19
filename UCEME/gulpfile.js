/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

// grab our packages
var gulp = require('gulp'),
    jshint = require('gulp-jshint'),
    jscs = require('gulp-jscs'),
    stylish = require('gulp-jscs-stylish');
var noop = function () { };

// define the default task and add the watch task to it
gulp.task('default-watch', ['watch']);

// configure the jshint task
gulp.task('jshint', function () {
    return gulp.src('scripts/internal/**/*.js')
      .pipe(jshint())
      .pipe(jshint.reporter('jshint-stylish'));
});

// configure which files to watch and what tasks to use on file changes
gulp.task('watch', function () {
    gulp.watch('scripts/internal/**/*.js', ['jshint']);
});

gulp.task('default', function () {
    return gulp.src('scripts/internal/**/*.js')
        .pipe(jscs({
            fix: true
        }))
        .pipe(gulp.dest('scripts/external'));
});

gulp.task('jscs-fix', function () {
    return gulp.src('scripts/internal/**/*.js')
        .pipe(jscs({
            'preset': 'google',
            fix: true
        }))
        .on('error', noop) // don't stop on error 
        .pipe(gulp.dest('scripts/internal'));
});

gulp.task('jscs', function () {
    return gulp.src('scripts/internal/**/*.js')
        .pipe(jscs())      // enforce style guide 
        .on('error', noop) // don't stop on error 
        .pipe(stylish());  // log style errors 
});