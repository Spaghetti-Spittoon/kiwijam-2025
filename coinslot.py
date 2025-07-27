import board
import analogio
import time
import usb_hid
from adafruit_hid.keyboard import Keyboard
from adafruit_hid.keycode import Keycode
import displayio
import terminalio
from adafruit_display_text.bitmap_label import Label
import busio
from fourwire import FourWire
from adafruit_gc9a01a import GC9A01A

# Setup
ldr = analogio.AnalogIn(board.GP28)  # Move LDR to GPIO28 to avoid SPI conflicts
kbd = Keyboard(usb_hid.devices)

# Setup display using SPI
print("Setting up display...")
displayio.release_displays()
spi = busio.SPI(board.GP2, MOSI=board.GP3)  # SCK=GP2, MOSI=GP3
tft_cs = board.GP5    # CS 
tft_dc = board.GP4    # DC
tft_reset = board.GP6 # RST (using GP6 instead of conflicting pins)

print("Creating display bus...")
display_bus = FourWire(spi, command=tft_dc, chip_select=tft_cs, reset=tft_reset)
print("Initializing GC9A01A...")
display = GC9A01A(display_bus, width=240, height=240)
display.rotation = 180
print("Display initialized!")

# Create display groups
main_group = displayio.Group()
display.root_group = main_group

# Background
bg_bitmap = displayio.Bitmap(240, 240, 1)
color_palette = displayio.Palette(1)
color_palette[0] = 0xFF0000 
bg_sprite = displayio.TileGrid(bg_bitmap, pixel_shader=color_palette)
main_group.append(bg_sprite)

# Coin count text  
count_group = displayio.Group(scale=12, x=90, y=118)
count_text = Label(terminalio.FONT, text="0", color=0x00FFFF)
count_group.append(count_text)
main_group.append(count_group)

# Settings
LIGHT_THRESHOLD = 30000  # Adjust based on your readings
DEBOUNCE_TIME = 0.3  # 300ms debounce

# State tracking
last_press_time = 0
was_dark = False
coin_count = 0

try:
    while True:
        light_level = ldr.value
        is_dark = light_level < LIGHT_THRESHOLD
        current_time = time.monotonic()
        
        if not is_dark and was_dark:
            time_since_last = current_time - last_press_time
            
            # Send key if debounce time has passed
            if time_since_last >= DEBOUNCE_TIME:
                coin_count += 1
                kbd.send(Keycode.C)
                last_press_time = current_time
                
                # Update display
                count_text.text = f"{coin_count}"
                
                if coin_count > 99:
                    count_group.x = 14
                elif coin_count > 9:
                    count_group.x = 54
        
        was_dark = is_dark
        time.sleep(0.01)  # 10ms polling

except KeyboardInterrupt:
    print("\nCleaning up...")
    try:
        ldr.deinit()
    except:
        pass
    print("Done! You can run the code again now.")
except Exception as e:
    print(f"Error: {e}")
    try:
        ldr.deinit()
    except:
        pass


