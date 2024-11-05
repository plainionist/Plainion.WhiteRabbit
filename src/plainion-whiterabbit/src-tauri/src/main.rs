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
                    "start_time": "2024-11-05T08:00:00Z",
                    "stop_time": "2024-11-05T09:30:00Z",
                    "comment": "Worked on Project A"
                },
                {
                    "start_time": "2024-11-05T10:00:00Z",
                    "stop_time": "2024-11-05T11:45:00Z",
                    "comment": "Code review and debugging"
                },
                {
                    "start_time": "2024-11-05T13:00:00Z",
                    "stop_time": "2024-11-05T14:30:00Z",
                    "comment": "Team meeting and planning"
                },
                {
                    "start_time": "2024-11-05T10:00:00Z",
                    "stop_time": "2024-11-05T11:45:00Z",
                    "comment": "Code review and debugging"
                },
                {
                    "start_time": "2024-11-05T13:00:00Z",
                    "stop_time": "2024-11-05T14:30:00Z",
                    "comment": "Team meeting and planning"
                },
                {
                    "start_time": "2024-11-05T10:00:00Z",
                    "stop_time": "2024-11-05T11:45:00Z",
                    "comment": "Code review and debugging"
                },
                {
                    "start_time": "2024-11-05T13:00:00Z",
                    "stop_time": "2024-11-05T14:30:00Z",
                    "comment": "Team meeting and planning"
                },
                {
                    "start_time": "2024-11-05T10:00:00Z",
                    "stop_time": "2024-11-05T11:45:00Z",
                    "comment": "Code review and debugging"
                },
                {
                    "start_time": "2024-11-05T13:00:00Z",
                    "stop_time": "2024-11-05T14:30:00Z",
                    "comment": "Team meeting and planning"
                },
                {
                    "start_time": "2024-11-05T15:00:00Z",
                    "stop_time": "2024-11-05T16:15:00Z",
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
