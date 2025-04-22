//
//  FixFloatWrapper.h
//  SnakeGameSingle
//
//  Created by karos li on 2025/4/22.
//  Copyright © 2025 WepieSnakeGame. All rights reserved.
//

#import <Foundation/Foundation.h>

// 使用前向声明减少耦合
#if defined(__cplusplus)
struct FixFloat; // C++ 前向声明
#else
typedef struct FixFloat FixFloat; // C 兼容声明
#endif

@interface FixFloatWrapper : NSObject

- (instancetype)initWithFixFloat:(FixFloat)ff;
- (FixFloat)getFixFloat;

@end
