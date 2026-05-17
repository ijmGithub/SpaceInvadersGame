import wave, struct, math
sample_rate = 44100
freqs = [220, 277, 330, 415, 330, 277, 220, 165]
notes = []
for f in freqs:
    for n in range(int(sample_rate * 0.5)):
        notes.append(int(0.18 * 32767 * math.sin(2 * math.pi * f * (n / sample_rate))))
for i in range(int(sample_rate * 2.0)):
    notes.append(int(0.12 * 32767 * math.sin(2 * math.pi * 55 * (i / sample_rate))))
for _ in range(3):
    for f in freqs:
        for n in range(int(sample_rate * 0.5)):
            notes.append(int(0.18 * 32767 * math.sin(2 * math.pi * f * (n / sample_rate))))
with wave.open('Resources/SpaceInvadersTheme.wav', 'w') as wav:
    wav.setnchannels(1)
    wav.setsampwidth(2)
    wav.setframerate(sample_rate)
    for v in notes:
        wav.writeframes(struct.pack('<h', max(-32768, min(32767, v))))
print('WAV generated:', len(notes), 'samples')
