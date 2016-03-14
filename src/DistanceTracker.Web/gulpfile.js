/// <binding BeforeBuild='dev' />

var gulp = require('gulp'),
    concat = require('gulp-concat-util'),
    merge = require('merge-stream'),
    angularFilesort = require('gulp-angular-filesort'),
    uglify = require('gulp-uglify'),
    cssmin = require("gulp-cssmin"),
    rename = require('gulp-rename'),
    del = require('del'),
    debug = require('gulp-debug'),
    jshint = require('gulp-jshint'),
    gulpif = require('gulp-if');

var paths = {
    webroot: "./wwwroot/"
};
paths.lib = paths.webroot + 'lib';

// Lib

gulp.task('clean:lib', function () {
    return del(paths.lib);
});

gulp.task('dev:lib', ['clean:lib'], function () {
    return gulp.src([
        'bower_components/**/dist/**/',
        'bower_components/angular*/angular*.js',
        'bower_components/angular*/ui-bootstrap-tpls.*',
        'bower_components/moment/moment.js',
        'bower_components/toastr*/*.*'
    ])
        .pipe(gulp.dest(paths.lib));
});

gulp.task('prod:lib', ['clean:lib'], function () {
    return gulp.src([
        'bower_components/**/dist/**/*.min.*',
        'bower_components/angular*/angular*.min.js',
        'bower_components/angular*/ui-bootstrap-tpls.min.js',
        'bower_components/moment/min/moment.min.js',
        'bower_components/toastr*/*.min.*'
    ])
        .pipe(gulp.dest(paths.lib));
});

// SPA

gulp.task('clean:spa', function () {
    return del([
        paths.webroot + 'app.js',
        paths.webroot + 'app.min.js',
        paths.webroot + 'templates'
    ]);
});

function getSpaStream(withJsHint) {
    var withJsHint = typeof withJsHint !== 'undefined' ? withJsHint : true;

    return gulp.src(['spa/app/**/*.js'])
        .pipe(angularFilesort())
        .pipe(gulpif(withJsHint, jshint()))
        .pipe(gulpif(withJsHint, jshint.reporter('default')))
        .pipe(concat('app.js'))
};

function getSpaStreamMin() {
    return getSpaStream()
        .pipe(uglify())
        .pipe(rename({
            suffix: '.min'
        }));
};

function getSpaTemplatesStream() {
    return gulp.src('spa/app/**/*.html')
        .pipe(gulp.dest(paths.webroot + '/templates'));
}

gulp.task('dev:spa', ['clean:spa'], function () {
    var js = merge(getSpaStream(false), getSpaStreamMin())
        .pipe(gulp.dest(paths.webroot));

    return merge(js, getSpaTemplatesStream());
});

gulp.task('prod:spa', ['clean:spa'], function () {
    var js = getSpaStreamMin()
        .pipe(gulp.dest(paths.webroot));

    return merge(js, getSpaTemplatesStream());
});

// Content

gulp.task('clean:content', function () {
    return del([
        paths.webroot + 'app.css',
        paths.webroot + 'app.min.css',
        paths.webroot + 'img',
    ]);
});

function getContentImgStream() {
    return gulp.src(['spa/content/**/*.png']);
}

function getContentCssStream() {
    return gulp.src(['spa/content/**/*.css'])
        .pipe(concat('app.css'));
}

function getContentCssStreamMin() {
    return getContentCssStream()
        .pipe(cssmin())
        .pipe(rename({
            suffix: '.min'
        }));
}

gulp.task('dev:content', ['clean:content'], function () {
    return merge(getContentCssStream(), getContentCssStreamMin(), getContentImgStream())
        .pipe(gulp.dest(paths.webroot));
});

gulp.task('prod:content', ['clean:content'], function () {
    return merge(getContentCssStreamMin(), getContentImgStream())
        .pipe(gulp.dest(paths.webroot));
});

// Main

gulp.task('watch', function () {
    gulp.watch('spa/**/*.*', ['dev']);
});

gulp.task('dev', ['dev:lib', 'dev:spa', 'dev:content'], function () {

});

gulp.task('prod', ['prod:lib', 'prod:spa', 'prod:content'], function () {

});

gulp.task('test', function () {

});

gulp.task('default', ['dev'], function () {

});