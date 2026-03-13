import os
import re

src_dir = r'C:\Users\Pierr\Documents\Projects\Decompiles\Minions_Paradise\SANDBOX\5-3-5p4-UP\ExportedProject\Assets\Shader\Shader'
dst_dir = r'C:\Users\Pierr\Documents\Projects\Decompiles\Minions_Paradise\SANDBOX\5-3-5p4-UP\ExportedProject\Assets\Shader'

# Pattern to find blocks like:
# Program "vp" { SubProgram "gles " { "..." } } Program "fp" { SubProgram "gles " { "" } }
# Actually, let's just replace all `Program "vp" { \n SubProgram "gles " { \n "` with `GLSLPROGRAM\n`
# But we need to handle the whole block carefully.
# The shader contains standard ShaderLab, but with `Program "vp" ...` instead of `CGPROGRAM` or `GLSLPROGRAM`.
# Let's extract the actual shader string.

def process_file(src_path, dst_path):
    with open(src_path, 'r', encoding='utf-8', errors='ignore') as f:
        content = f.read()
    
    # Remove the dummy notice
    content = re.sub(r'//+[\s]*\n//\s*NOTE: This is \*not\* a valid shader file\s*\n//+[\s]*\n', '', content)
    
    # Remove GpuProgramID
    content = re.sub(r'\s*GpuProgramID \d+', '', content)

    # find all Program "vp" { SubProgram "xxx" { "..." } } Program "fp" { SubProgram "xxx" { "" } }
    # Sometimes it's one block, sometimes multiple passes. 
    # Let's use a regex that captures the string inside the vp subprogram.
    # We look for Program "vp" ... string ... and ignore fp string.
    
    def strip_quotes(s):
        if s.startswith('"') and s.endswith('"'):
            s = s[1:-1]
        # Replace literal \n with actual newlines if present, but usually it's just a multi-line string in quotes
        s = s.replace('\\n', '\n')
        return s

    # Regex to capture the whole Program "vp" ... Program "fp" ... block
    # It starts with Program "vp" { and ends with the matching } of Program "fp".
    # This might be tricky with regex if there are nested braces inside the string, but shader strings in these dumps don't typically have them unescaped.
    # Actually, simpler: just regex to find SubProgram "[a-zA-Z0-9 ]+" { \n "(.*?)" \n }
    # Wait, the string is multiline and bounded by quotes at the start and end.
    
    # Let's find: `Program "vp" \{.*?SubProgram.*?\{\s*"(.*?)"\s*\}\s*\}\s*Program "fp" \{.*?SubProgram.*?\{\s*""\s*\}\s*\}`
    # Wait, fp might not be empty. What if fp has the fragment shader?
    # In GLES, both vertex and fragment are in the same source! (using #ifdef VERTEX / #ifdef FRAGMENT).
    # That's why fp is empty!
    
    pattern = re.compile(
        r'Program "vp"\s*\{\s*SubProgram "[^"]*?"\s*\{\s*"(.*?)"\s*\}\s*\}\s*Program "fp"\s*\{\s*SubProgram "[^"]*?"\s*\{\s*""\s*\}\s*\}',
        re.DOTALL
    )
    
    def replacer(m):
        shader_body = m.group(1)
        # Unescape quotes if needed
        shader_body = shader_body.replace('\\"', '"').replace('\\n', '\n')
        return "GLSLPROGRAM\n" + shader_body + "\nENDGLSL"
    
    new_content = pattern.sub(replacer, content)
    
    with open(dst_path, 'w', encoding='utf-8') as f:
        f.write(new_content)

for file_name in os.listdir(src_dir):
    if not file_name.endswith('.shader'): continue
    src_path = os.path.join(src_dir, file_name)
    dst_path = os.path.join(dst_dir, file_name)
    process_file(src_path, dst_path)

print("Done")
