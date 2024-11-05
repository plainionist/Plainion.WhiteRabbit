// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use serde_json::json;
use tauri_dotnet_bridge_host;
#[tauri::command]
fn dotnet_request(request: &str) -> String {
    //tauri_dotnet_bridge_host::process_request(request)

    println!("Request: {}", request);

    let data = json!(
        {
            "data": [
                {
                    "start": "08:00:00",
                    "stop": "09:30:00",
                    "comment": "Worked on Project A"
                },
                {
                    "start": "10:00:00",
                    "stop": "11:45:00",
                    "comment": "Code review and debugging"
                },
                {
                    "start": "13:00:00",
                    "stop": "14:30:00",
                    "comment": "Team meeting and planning"
                },
                {
                    "start": "10:00:00",
                    "stop": "11:45:00",
                    "comment": "Code review and debugging"
                },
                {
                    "start": "13:00:00",
                    "stop": "14:30:00",
                    "comment": "Team meeting and planning"
                },
                {
                    "start": "10:00:00",
                    "stop": "11:45:00",
                    "comment": "Code review and debugging"
                },
                {
                    "start": "13:00:00",
                    "stop": "14:30:00",
                    "comment": "Team meeting and planning"
                },
                {
                    "start": "10:00:00",
                    "stop": "11:45:00",
                    "comment": "Code review and debugging"
                },
                {
                    "start": "13:00:00",
                    "stop": "14:30:00",
                    "comment": "Team meeting and planning"
                },
                {
                    "start": "15:00:00",
                    "stop": "16:15:00",
                    "comment": "Worked on UI improvements"
                }
            ]
        }
    );

    data.to_string()
}

fn main() {
    tauri::Builder::default()
        .plugin(tauri_plugin_shell::init())
        .invoke_handler(tauri::generate_handler![dotnet_request])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
