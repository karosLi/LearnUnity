//
//  MathUtils.h
//  SnakeGameSingle
//
//  Created by ZhuGuangwen on 16/8/5.
//  Copyright © 2016年 ZhuGuangwen. All rights reserved.
//

#import <UIKit/UIKit.h>

#define GET_VALID_DIR(dir) ({\
dir = dir > ((float)M_PI) ? dir - ((float)M_PI) * 2 : dir;\
dir = dir <= -((float)M_PI) ? dir + ((float)M_PI) * 2 : dir;\
dir;\
});

struct MathLine {
    OLPoint start;
    OLPoint end;
};
typedef struct MathLine MathLine;
static inline MathLine MathLineMake(OLPoint start, OLPoint end)
{
    MathLine line; line.start = start; line.end = end; return line;
}

@interface NSValue (MathLine)
+ (instancetype)valueWithLine:(MathLine)value;
@property (readonly) MathLine lineValue;
@end

static BOOL useRandSeed = YES;

@interface MathUtils : NSObject

+ (NSString *)convertFloatToString:(float)value;
+ (void)srand:(int)seed;
+ (int)fand;
+ (NSInteger)fandTimes;
+ (NSInteger)lastFandResult;
+ (uint32_t)arc4random;

+ (OLFloat)randomWithMin:(OLFloat)min max:(OLFloat)max;
+ (OLFloat)arc4randomWithMin:(OLFloat)min max:(OLFloat)max;

+ (OLFloat)randomDirectionWithDirection:(OLFloat)direction offset:(OLFloat)offset;

+ (NSInteger)randomDegree;

+ (OLFloat)directionWithDegree:(NSInteger)degree;

+ (OLFloat)distanceWithX1:(OLFloat)x1 y1:(OLFloat)y1 x2:(OLFloat)x2 y2:(OLFloat)y2;
+ (OLFloat)distanceWithPoint1:(OLPoint)point1 point2:(OLPoint)point2;

+ (OLFloat)squareDistanceWithX1:(OLFloat)x1 y1:(OLFloat)y1 x2:(OLFloat)x2 y2:(OLFloat)y2;
+ (OLFloat)squareDistanceWithPoint1:(OLPoint)point1 point2:(OLPoint)point2;

+ (OLFloat)minDirectionBetweenDirection1:(OLFloat)direction1 direction2:(OLFloat)direction2;
+ (OLFloat)directionFromDirection:(OLFloat)direction toDirection:(OLFloat)toDirection limit:(OLFloat)limit;

+ (OLPoint)randomPointInRect:(OLRect)rect;
+ (OLPoint)randomPointInnerRect:(OLRect)innerRect outerRect:(OLRect)outerRect;

+ (NSInteger)encryptNumberWithNumber:(NSInteger)number;
+ (NSInteger)decryptNumberWithNumber:(NSInteger)number;

/// 以矩形来构建一个4个线段
+ (NSArray<NSValue *> *)linesWithRect:(OLRect)rect padding:(OLSize)padding;
/// 以中心点和大小来构建一个4个线段
+ (NSArray<NSValue *> *)linesWithCenter:(OLPoint)center size:(OLSize)size padding:(OLSize)padding;

/// 根据两个矩形，通道宽度和通道长度获取两个矩形之间连接的矩形通道
+ (OLRect)pipeRectWithRect:(OLRect)rect otherRect:(OLRect)other pipeDiameter:(OLFloat)pipeDiameter pipeLength:(OLFloat)pipeLength;

// point1和point2组成线段line1，point3和point4组成线段line2，如果line1和line2相交，则返回YES，否则NO
+ (BOOL)hasLineCrossingWithPoint1:(OLPoint)point1 point2:(OLPoint)point2 point3:(OLPoint)point3 point4:(OLPoint)point4 interPoint:(OLPoint *)interPoint;

+ (OLFloat)lerpValueWithRenderIndex:(int)renderIndex
                    animationValues:(OLFloat *)animationValues
                 animationDurations:(OLFloat *)animationDurations
                     durationLength:(int)durationLength
                       defaultValue:(OLFloat)defaultValue;

+ (OLFloat)distanceWithPoint:(OLPoint)point lineP1:(OLPoint)p1 lineP2:(OLPoint)p2 interPoint:(OLPoint *)interPoint;

/// 线性插值：获取某一帧的中间状态的值

/// 举个例子，这个两个数组表示，scale 会从 0.0 动画(动画时间0.16s)到 1.05，然后从 1.05 动画(动画时间0.08s)到 0.98，然后从 0.98 动画(动画时间0.08s)到 1.0
/// GLfloat scaleArr[] = {0.0, 1.05, 0.98, 1.0};
/// GLfloat durationArr[] = {0.16, 0.08, 0.08};
///
/// @param renderIndex 当前帧
/// @param animationValues 动画值的变化数组
/// @param animationDurations 每个变化的动画持续时间
/// @param durationLength 动画持续时间的长度
/// @param defaultValue 超出动画时间的范围后需要使用的默认值
/// @param reverseAnimation 是否需要倒放动画
/// @param animationCompleted 动画是否播放完成
/// @param animationRepeat 动画是否需要重复播放
+ (OLFloat)lerpValueWithRenderIndex:(int)renderIndex
                    animationValues:(OLFloat *)animationValues
                 animationDurations:(OLFloat *)animationDurations
                     durationLength:(int)durationLength
                       defaultValue:(OLFloat)defaultValue
                   reverseAnimation:(BOOL)reverseAnimation
                 animationCompleted:(BOOL *)animationCompleted
                    animationRepeat:(BOOL)animationRepeat;

/// 点是否在 rect 里面
+ (BOOL)point:(OLPoint)point inRect:(OLRect)rect;

/// 向量单位化
+ (OLPoint)normalizeVector:(OLPoint)vector;
/// 找到一个rect范围内的安全点
+ (OLPoint)safePoint:(OLPoint)point inRect:(OLRect)rect padding:(OLSize)padding;
// 在回字形内随机一个点
+ (OLPoint)randomPointInRect:(OLRect)rect rectPadding:(OLSize)rectPadding excludeRect:(OLRect)excludeRect excludeRectPadding:(OLSize)excludeRectPadding;
// 简单的线性插值
+ (OLFloat)lerpFrom:(OLFloat)startValue to:(OLFloat)endValue atTime:(OLFloat)currentTime overDuration:(OLFloat)duration;

+ (float)cos:(float)number;
+ (float)sin:(float)number;
+ (float)tan:(float)number;
+ (float)atan2f:(float)y x:(float)x;

@end
