# Greeting
Welcome to the README of my application called Transfer. This is a multimedia application for viewing videos with the ability to edit or convert them into different formats, change the speed, and some other features.

## About the Program
This program was created by me for my personal use and is not intended for use on operating systems other than Linux and Windows. You can compile the program for Mac yourself, but I cannot guarantee proper functionality. The program uses FFmpeg for video editing, specifically the <a href="https://github.com/rosenbjerg/FFMpegCore">FFMpegCore</a> library with the ability to add your own arguments, and osu-framework for creating the shell. osu-framework provides many tools for working with the window and other means.

### About the Developer
I am slepming, greetings to everyone who has read this README. I live and study in Russia and cannot make regular commits to the repository due to my studies. I maintain this application purely out of my own interest; I love writing code regardless of the task. All tasks are derived from my needs, my principle is "If you don't like it, change it." I won't have much time and may soon get tired of this project, but at the moment, I enjoy writing it and learning about FFMpeg and <a href="https://github.com/ppy/osu-framework">osu-framework</a>. I have a weak laptop and try to optimize the application for use on my own laptop. If you find the cause of any lag in my application, please fix it; you will be a great help to me.

### Building
You can compile the application using `dotnet build -c Release` in the folder with Transfer, or for testing, you can go to Transfer.Game.Tests and run `dotnet watch`. osu-framework works well with Hot reload, so you can edit lines of code in real-time and see the changes.
