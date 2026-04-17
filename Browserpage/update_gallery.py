import os
import json

def update_gallery():
    screenshots_dir = 'GameScreenshots'
    gallery_file = 'gallery.json'
    
    # Supported image extensions
    valid_extensions = ('.png', '.jpg', '.jpeg', '.webp')
    
    gallery_data = []
    
    # Check if directory exists
    if not os.path.exists(screenshots_dir):
        print(f"Error: Directory '{screenshots_dir}' not found.")
        return

    # List files in the screenshots directory
    files = os.listdir(screenshots_dir)
    # Sort files to maintain consistent order
    files.sort()
    
    for filename in files:
        if filename.lower().endswith(valid_extensions):
            image_path = f"{screenshots_dir}/{filename}"
            base_name = os.path.splitext(filename)[0]
            desc_file = f"{screenshots_dir}/{base_name}.desc"
            
            # Check if desc file exists
            if not os.path.exists(desc_file):
                # Create an empty desc file if it doesn't exist
                with open(desc_file, 'w', encoding='utf-8') as f:
                    f.write(f"{base_name.replace('_', ' ').capitalize()}\nAuthor\n{filename}")
                print(f"Created missing description file: {desc_file}")
            
            gallery_data.append({
                "image": image_path,
                "descFile": desc_file
            })
    
    # Write to gallery.json
    with open(gallery_file, 'w', encoding='utf-8') as f:
        json.dump(gallery_data, f, indent=2)
    
    print(f"Successfully updated {gallery_file} with {len(gallery_data)} images.")

if __name__ == "__main__":
    update_gallery()
