# Set the folder where your MAUI app was published
publish_folder <- r"(C:\Code\MAUI\PTANonCrown\PTANonCrown\bin\x64\Release\net8.0-windows10.0.19041.0\win-x64\publish)"  # replace with your path

# Output file (inside the publish folder)
output_file <- file.path(publish_folder, "PTANonCrownFiles.wxs")

# List all files recursively
all_files <- list.files(publish_folder, recursive = TRUE, full.names = TRUE)

# Open connection to write
con <- file(output_file, open = "w", encoding = "UTF-8")

# Counter for suffix
counter <- 1

for (f in all_files) {
  file_name <- basename(f)
  # Make safe WiX Id
  base_id <- gsub("[^A-Za-z0-9]", "_", file_name)
  # Add numeric suffix
  file_id <- sprintf("%s_%03d", base_id, counter)
  counter <- counter + 1
  
  # Write <File> tag
  cat(sprintf('<File Id="%s" Source="%s" />\n', file_id, f), file = con)
}

close(con)

cat("WiX <File> entries generated at:", output_file, "\n")
