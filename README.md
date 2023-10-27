
# Cyber Scope 🖥️
This project involves the development of a simulated remote access trojan (RAT) to serve as a proof-of-concept or for educational purposes. It employs C# for the client-side and PHP for the server-side. Please note that this tool is meant for ethical and research uses only. Any misuse of this software will be solely the user's responsibility.

## Features 🌟
- Client-side in C#: Robust client application written in C#.
- Server-side in PHP: Efficient server-side scripting using PHP.
- Interactive User Interface: The client comes with a UI that offers:
- Execution of commands.
- Retrieval of command results.
- Capturing the victim's desktop screenshot.
- Gathering key logs from the victim's machine.
- MySQL Integration: Efficiently stores and manages data from multiple clients using a MySQL database. This ensures organized data storage and easy retrieval.

## Setup & Installation 🔧
1. **Server Setup:**
- Deploy the PHP scripts to your web server.
- Setup a MySQL database and modify the PHP scripts to connect to your database.
2. **Client Setup:
- Compile the C# source code.
- Ensure the compiled application points to your PHP server.

## Usage 📖
1. **Server-side:**
- Nabigate to your web server the PHP scripts are hosted.
- Monitor incoming client connections and manage the stored data.
2. **Client-side:**
- Execute the compiled C# application.
- Use the user interface to send commands to the server, retrieve command results, capture screenshots, and gather key logs.


