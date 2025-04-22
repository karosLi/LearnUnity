//
//  FixFloatWrapper.m
//  SnakeGameSingle
//
//  Created by karos li on 2025/4/22.
//  Copyright © 2025 WepieSnakeGame. All rights reserved.
//

#import "FixFloatWrapper.h"
#import "FixFloat.hpp"

@implementation FixFloatWrapper {
    FixFloat _fixFloat; // 直接包含 C++ 成员
}

- (instancetype)initWithFixFloat:(FixFloat)ff {
    if (self = [super init]) {
        _fixFloat = ff;
    }
    return self;
}

- (FixFloat)getFixFloat {
    return _fixFloat;
}

@end
