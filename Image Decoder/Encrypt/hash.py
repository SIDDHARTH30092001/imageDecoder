from PIL import Image
import hashlib
from hashlib import sha1

f = open("D:\Encrypt\ImagePath.txt", "r")
path=f.read()
f.close()

md5hash = hashlib.md5(Image.open(path).tobytes())
f3= open("D:\Encrypt\MD5.txt", "w")
f3.write(md5hash.hexdigest())
f3.close()

sha1hash = sha1(open(path, 'rb').read()).hexdigest()
f4= open("D:\Encrypt\SHA1.txt", "w")
f4.write(sha1hash)
f4.close()
