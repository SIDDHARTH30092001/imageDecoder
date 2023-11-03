import cv2
import numpy as np
from PIL import Image
import hashlib
from hashlib import sha1

f = open("D:\Encrypt\ImagePath.txt", "r")
path=f.read()
f.close()
f1=open("D:\Encrypt\ImageExt.txt", "r")
ext=f1.read()
f1.close()

demo = cv2.imread(path, 0)
r, c = demo.shape

key = np.random.randint(0, 256, size=(r, c), dtype=np.uint8)  # Generate random key image
cv2.imwrite("D:\Encrypt\key_"+ext, key)   # Save key image

encryption = cv2.bitwise_xor(demo, key)  # encryption
cv2.imwrite("D:\Encrypt\enc_"+ext, encryption)     # Save the encrypted image

decryption = cv2.bitwise_xor(encryption, key)  # decrypt
cv2.imwrite("D:\Encrypt\dec_"+ext, decryption) # Save the decrypted image
