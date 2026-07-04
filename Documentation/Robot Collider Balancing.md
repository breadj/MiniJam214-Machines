# Blue
Circle collider: { radius = 0.44 }
Area = pi \* 0.44^2
	= **pi \* 0.1936**
# Red
Capsule collider: { x = 1.2, y = 0.65 }
diameter = 0.65
shaft = 1.2 - 0.65
	= 0.55
radius = 0.65 / 2
	= 0.325
Area = 0.65 \* 0.55 + pi \* 0.325^2
	= **0.3575 + pi \* 0.105625**

# Difference
Blue Area = pi \* 0.1936
Red Area = 0.3575 + pi \* 0.105625

BA \* k = RA
pi \* 0.1936 \* k = 0.3575 + pi \* 0.105625
pi \* 0.1936 \* k - pi \* 0.105625 = 0.3575
pi \* (0.1936 \* k - 0.105625) = 0.3575
0.1936 \* k - 0.105625 = 0.3575 / pi
0.1936 \* k = 0.3575 / pi + 0.105625
k = (0.3575 / 0.1936) / pi + 0.105625 / 0.1936
k ≅ **1.133** (3.d.p.)

Red Area is approx. **1.133x** larger than Blue Area.
Blue Area is approx. **0.882x** (3.d.p.) larger than Red Area.
#### **Therefore set Red robot's scale to 0.882.**