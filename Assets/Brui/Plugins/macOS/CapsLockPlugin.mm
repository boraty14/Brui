#import <Foundation/Foundation.h>
#import <Carbon/Carbon.h>

extern "C" {
    bool IsCapsLockOnMac() {
        NSUInteger flags = [NSEvent modifierFlags] & NSEventModifierFlagDeviceIndependentFlagsMask;
        return (flags & NSEventModifierFlagCapsLock) != 0;
    }
}