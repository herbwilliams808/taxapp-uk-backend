import json
import os
import sys

def convert_to_camel_case(text):
    return ''.join(word.capitalize() for word in text.split())

def main(payload_path):
    with open(payload_path, 'r') as f:
        print("Payload: " + f)
        data = json.load(f)

    summary = data.get('summary', 'UnnamedFeature')
    description = data.get('description', '')

    # Ensure the directory exists
    os.makedirs('Requirements/Features', exist_ok=True)

    filename = convert_to_camel_case(summary) + ".feature"
    filepath = os.path.join('Requirements/Features', filename)

    with open(filepath, 'w') as f:
        f.write(description)

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python3 create_feature_file.py <payload_path>")
        sys.exit(1)

    main(sys.argv[1])
