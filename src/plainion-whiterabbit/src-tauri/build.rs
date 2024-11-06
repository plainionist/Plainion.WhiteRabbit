use std::process::Command;

fn main() {
    println!("cargo:rerun-if-changed=../src-dotnet");

    Command::new("dotnet")
        .arg("build")
        .current_dir("../src-dotnet")
        .status()
        .expect("Failed to run dotnet build");

    tauri_build::build()
}
