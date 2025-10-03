# to-do-app

This is a simple To-Do list application I built in C# with WPF.  
It was mainly a practice project to work on desktop app development, saving data, and handling basic UI functionality in WPF/XAML.  

## Features
- Add, edit, and delete tasks
- Title, description, and optional due date/time for each task
- Persistent storage (tasks are saved to a JSON file so they load again when the app restarts)
- Mark tasks as complete/incomplete with a checkbox
- Basic UI improvements (rounded buttons, cleaner styling)

## Controls / Usage
- **Add Task** — Opens the input panel, fill out details, then click `Add`
- **Edit Task** — Select a task from the list, click `Edit Task`, make changes, then click `Save`
- **Delete Task** — Select a task and click `Delete Task`
- **Mark Complete** — Use the checkbox beside a task to mark it complete/incomplete

## Things I Learned
- How to wire up WPF UI elements to C# code-behind
- How to save and load data from JSON for persistence
- How to handle input validation and nullable values (like optional due dates)
- What a pain XAML is

## Comments
This is version **1.0** — the goal was just to have a functional, working app.  
I may come back later to:
- Add filtering/sorting (e.g., show only incomplete tasks, or sort by due date)
- Improve UI with something like Material Design styles
- Inline editing (instead of a separate input panel)
- Notifications/reminders for upcoming tasks

For now, this is a very basic but functional To-Do app that demonstrates core CRUD (Create, Read, Update, Delete) functionality in WPF.
