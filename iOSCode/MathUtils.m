//
//  MathUtils.m
//  SnakeGameSingle
//
//  Created by ZhuGuangwen on 16/8/5.
//  Copyright © 2016年 ZhuGuangwen. All rights reserved.
//

#import "SNKSGGameContext.h"
#import "SNKSGGameManager.h"
#import "SNKSgGameFileLogTool.h"

@implementation NSValue (MathLine)
+ (instancetype)valueWithLine:(MathLine)value
{
    return [self valueWithBytes:&value objCType:@encode(MathLine)];
}
- (MathLine)lineValue
{
    MathLine value;
    [self getValue:&value];
    return value;
}
@end

@implementation MathUtils

static NSInteger fandTimes = 0;
static int fandResult = 0;

static unsigned int g_seed = 0;

+ (NSString *)convertFloatToString:(float)value {
    int decimalPlaces = 8;
    
    // 处理负数情况
    BOOL isNegative = value < 0;
    if (isNegative) {
        value = -value; // 转为正数处理
    }

    // 提取整数部分
    int integralPart = (int)value;
    // 提取小数部分
    float fractionalPart = value - integralPart;
    // 开始构建结果字符串，添加负号（如果有）
    NSMutableString *result = [NSMutableString stringWithFormat:@"%@%d.", (isNegative ? @"-" : @""), integralPart];

    // 处理小数部分
    for (int i = 0; i < decimalPlaces; i++) {
        fractionalPart *= 10;
        int digit = (int)fractionalPart;
        [result appendFormat:@"%d", digit];
        fractionalPart -= digit;
    }
    
    return [result stringByPaddingToLength:15 withString:@" " startingAtIndex:0];
}

+ (void)srand:(int)seed
{
    if (useRandSeed) {
        fandTimes = 0;
        g_seed = seed;
    }
}

+ (void)fandToTimes:(NSInteger)times withSeed:(int)seed
{
    NSInteger diff = times - fandTimes;
    if (diff > 0) {
        for (NSInteger i = 0;i < diff;i ++) {
            fandTimes ++;
            rand_r(&g_seed);
        }
    } else {
        NSAssert(NO, @"should not should be here");
        [self srand:seed];
        for (NSInteger i = 0;i < times;i ++) {
            fandTimes ++;
            rand_r(&g_seed);
        }
    }
}

+ (int)fand
{
    if ([SNKSGGameContext sharedContext].gameManager.renderIndex == 1886) {
        NSString *st = [[NSThread callStackSymbols] componentsJoinedByString:@"\n"];
        [[SNKSgGameFileLogTool sharedLogTool] writeStackTraceMsg:[NSString stringWithFormat:@"%@\n\n", st]];
    }
    
    fandTimes++;
    int fand = rand_r(&g_seed);
    fandResult = fand < 0 ? - fand : fand;
    return fandResult;
}

+ (NSInteger)fandTimes
{
    return fandTimes;
}

+ (NSInteger)lastFandResult
{
    return fandResult;
}

+ (uint32_t)arc4random {
    if (useRandSeed) {
        return [self fand];
    }
    
    return arc4random();
}

+ (OLFloat)randomWithMin:(OLFloat)min max:(OLFloat)max
{
    NSInteger m = 1000000;
    OLFloat random = ([MathUtils arc4random] % m) * 1.0f / m;
    return min + (max - min) * random;
}

+ (OLFloat)arc4randomWithMin:(OLFloat)min max:(OLFloat)max
{
    NSInteger m = 1000000;
    OLFloat random = (arc4random() % m) * 1.0f / m;
    return min + (max - min) * random;
}

+ (OLFloat)randomDirectionWithDirection:(OLFloat)direction offset:(OLFloat)offset
{
    OLFloat newDirection = [self randomWithMin:direction - offset max:direction + offset];
    return fmod(newDirection + M_PI, 2 * M_PI) - M_PI;
}

+ (NSInteger)randomDegree
{
    NSInteger random = [MathUtils arc4random] % 360;
    return random - 179;
}

+ (OLFloat)directionWithDegree:(NSInteger)degree
{
    NSAssert(degree > -180 && degree <= 180, @"invalid degree");
    return degree * M_PI / 180;
}

+ (OLFloat)distanceWithX1:(OLFloat)x1 y1:(OLFloat)y1 x2:(OLFloat)x2 y2:(OLFloat)y2
{
//    return sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    return sqrtf((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
}

+ (OLFloat)distanceWithPoint1:(OLPoint)point1 point2:(OLPoint)point2
{
//    return sqrt((point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y));
    return sqrtf((point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y));
}

+ (OLFloat)squareDistanceWithX1:(OLFloat)x1 y1:(OLFloat)y1 x2:(OLFloat)x2 y2:(OLFloat)y2
{
    return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
}

+ (OLFloat)squareDistanceWithPoint1:(OLPoint)point1 point2:(OLPoint)point2
{
    return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
}


+ (OLFloat)minDirectionBetweenDirection1:(OLFloat)direction1 direction2:(OLFloat)direction2
{
    if (fabs(direction1 - direction2) <= (float)M_PI) {
        return fabs(direction1 - direction2);
    } else {
        direction1 = direction1 > 0 ? direction1 : direction1 + ((float)M_PI) * 2;
        direction2 = direction2 > 0 ? direction2 : direction2 + ((float)M_PI) * 2;
        return fabs(direction1 - direction2);
    }
}

+ (OLFloat)directionFromDirection:(OLFloat)direction toDirection:(OLFloat)toDirection limit:(OLFloat)limit
{
    OLFloat delta = [self minDirectionBetweenDirection1:direction direction2:toDirection];
    if (delta <= limit) {
        return toDirection;
    }
    OLFloat dest1 = direction + limit;
    dest1 = GET_VALID_DIR(dest1);
    OLFloat dest2 = direction - limit;
    dest2 = GET_VALID_DIR(dest2);
    OLFloat ret = [self minDirectionBetweenDirection1:dest1 direction2:toDirection] < [self minDirectionBetweenDirection1:dest2 direction2:toDirection] ? dest1 : dest2;
    
//    printf("ret:%f direction:%f toDirection:%f\n", ret * 180.0 / M_PI, direction * 180.0 / M_PI, toDirection * 180.0 / M_PI);
    
    return ret;
}

+ (OLPoint)randomPointInRect:(OLRect)rect
{
    OLFloat x = [self randomWithMin:0 max:rect.size.width];
    OLFloat y = [self randomWithMin:0 max:rect.size.height];
    return OLPointMake(rect.origin.x + x, rect.origin.y + y);
}

+ (OLPoint)randomPointInnerRect:(OLRect)innerRect outerRect:(OLRect)outerRect
{
    OLFloat gapWidth = outerRect.size.width - innerRect.size.width;
    OLFloat x = [self randomWithMin:0 max:gapWidth];
    if (x < innerRect.origin.x - outerRect.origin.x) {
        x = outerRect.origin.x + x;
    } else {
        x = outerRect.origin.x + x + innerRect.size.width;
    }
    
    OLFloat gapHeight = outerRect.size.height - innerRect.size.height;
    OLFloat y = [self randomWithMin:0 max:gapHeight];
    if (y < innerRect.origin.y - outerRect.origin.y) {
        y = outerRect.origin.y + y;
    } else {
        y = outerRect.origin.y + y + innerRect.size.height;
    }
    
    return OLPointMake(x, y);
}

+ (NSInteger)encryptNumberWithNumber:(NSInteger)number
{
    return number * self.mulFactor + self.addFactor;
}

+ (NSInteger)decryptNumberWithNumber:(NSInteger)number
{
    return (number - self.addFactor) / self.mulFactor;
}

+ (NSInteger)mulFactor
{
    static NSInteger mulFactor;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        mulFactor = (arc4random() % 10) + 5;
    });
    return mulFactor;
}

+ (NSInteger)addFactor
{
    static NSInteger addFactor;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        addFactor = arc4random() % 1000;
    });
    return addFactor;
}

/// 以矩形来构建一个4个线段
+ (NSArray<NSValue *> *)linesWithRect:(OLRect)rect padding:(OLSize)padding {
    OLRect mapRect = rect;
    OLRect padingRect = OLRectInset(mapRect, padding.width, padding.height);
    
    OLFloat left = OLRectGetMinX(padingRect);
    OLFloat top = OLRectGetMaxY(padingRect);
    OLFloat right = OLRectGetMaxX(padingRect);
    OLFloat bottom = OLRectGetMinY(padingRect);
    
    // 从左下开始顺时针的四个角ABCD
    // B C
    // A D
    OLPoint A = OLPointMake(left, bottom);
    OLPoint B = OLPointMake(left, top);
    OLPoint C = OLPointMake(right, top);
    OLPoint D = OLPointMake(right, bottom);
    
    return @[[NSValue valueWithLine:MathLineMake(A, B)],
             [NSValue valueWithLine:MathLineMake(B, C)],
             [NSValue valueWithLine:MathLineMake(C, D)],
             [NSValue valueWithLine:MathLineMake(D, A)]
    ];
}

/// 以中心点和大小来构建一个4个线段
+ (NSArray<NSValue *> *)linesWithCenter:(OLPoint)center size:(OLSize)size padding:(OLSize)padding {
    OLRect rect = OLRectMake(center.x - size.width / 2.0, center.y - size.height / 2.0, size.width, size.height);
    OLRect padingRect = OLRectInset(rect, padding.width, padding.height);
    
    OLFloat left = OLRectGetMinX(padingRect);
    OLFloat top = OLRectGetMaxY(padingRect);
    OLFloat right = OLRectGetMaxX(padingRect);
    OLFloat bottom = OLRectGetMinY(padingRect);
    
    // 从左下开始顺时针的四个角ABCD
    // B C
    // A D
    OLPoint A = OLPointMake(left, bottom);
    OLPoint B = OLPointMake(left, top);
    OLPoint C = OLPointMake(right, top);
    OLPoint D = OLPointMake(right, bottom);
    
    return @[[NSValue valueWithLine:MathLineMake(A, B)],
             [NSValue valueWithLine:MathLineMake(B, C)],
             [NSValue valueWithLine:MathLineMake(C, D)],
             [NSValue valueWithLine:MathLineMake(D, A)]
    ];
}

+ (OLRect)pipeRectWithRect:(OLRect)rect otherRect:(OLRect)other pipeDiameter:(OLFloat)pipeDiameter pipeLength:(OLFloat)pipeLength {
    
    // 通道宽度为0时返回空值
    if (pipeDiameter <= 0) {
        return OLRectZero;
    }
    
    // 初始化基础数据
    OLFloat left = OLRectGetMinX(rect);
    OLFloat top = OLRectGetMaxY(rect);
    OLFloat right = OLRectGetMaxX(rect);
    OLFloat bottom = OLRectGetMinY(rect);
    
    OLFloat otherX = other.origin.x;
    OLFloat otherY = other.origin.y;
    
    OLFloat centerX = OLRectGetMidX(rect);
    OLFloat centerY = OLRectGetMidY(rect);
    OLFloat halfPipeDiameter = pipeDiameter / 2.0;
    
    // 根据位置返回数据
    if (otherX > right) { // 说明other在rect右边
        return OLRectMake(right, centerY - halfPipeDiameter, pipeLength, pipeDiameter);
    } else if (otherX < left) { // 说明other在rect左边
        return OLRectMake(left - pipeLength, centerY - halfPipeDiameter, pipeLength, pipeDiameter);
    } else if (otherY > top) { // 说明other在rect上边
        return OLRectMake(centerX - halfPipeDiameter, top, pipeDiameter, pipeLength);
    } else if (otherY < bottom) { // 说明other在rect下边
        return OLRectMake(centerX - halfPipeDiameter, bottom - pipeLength, pipeDiameter, pipeLength);
    }
    return OLRectZero;
}

// 参考链接： http://www-cs.ccny.cuny.edu/~wolberg/capstone/intersection/Intersection%20point%20of%20two%20lines.html
+ (BOOL)hasLineCrossingWithPoint1:(OLPoint)point1 point2:(OLPoint)point2 point3:(OLPoint)point3 point4:(OLPoint)point4 interPoint:(OLPoint *)interPoint {
    OLFloat u1 = ((point4.x - point3.x) * (point1.y - point3.y) - (point4.y - point3.y) * (point1.x - point3.x)) / ((point4.y - point3.y) * (point2.x - point1.x) - (point4.x - point3.x) * (point2.y - point1.y));
    OLFloat u2 = ((point2.x - point1.x) * (point1.y - point3.y) - (point2.y - point1.y) * (point1.x - point3.x)) / ((point4.y - point3.y) * (point2.x - point1.x) - (point4.x - point3.x) * (point2.y - point1.y));
    if (u1 >= 0 && u1 <= 1 && u2 >= 0 && u2 <= 1) {
        if (interPoint) {
            (*interPoint).x = point1.x + u1 * (point2.x - point1.x);
            (*interPoint).y = point1.y + u1 * (point2.y - point1.y);
        }
        return YES;
    }
    
    return NO;
}

/**
 点到直线的距离
 
 参数计算：
 A=y2-y1；
 B=x1-x2；
 C=x2y1-x1y2;
 1.点到直线的距离公式：
 d= ( Ax0 + By0 + C ) / sqrt ( AA + BB );
 2.垂足C（x，y）计算公式：
 x = ( BBx0 - ABy0 - AC ) / ( AA + BB );
 y = ( -ABx0 + AAy0 – BC ) / ( AA + BB );
 */
+ (OLFloat)distanceWithPoint:(OLPoint)point lineP1:(OLPoint)p1 lineP2:(OLPoint)p2 interPoint:(OLPoint *)interPoint {
    OLFloat a = p2.y - p1.y;
    OLFloat b = p1.x - p2.x;
    OLFloat c = p2.x * p1.y - p1.x * p2.y;
    
    OLFloat x = (b * b * point.x - a * b * point.y - a * c) / (a * a + b * b);
    OLFloat y = (-a * b * point.x + a * a * point.y - b * c) / (a * a + b * b);
    
//    OLFloat d = fabsf((a * point.x + b * point.y + c) / sqrtf(pow(a, 2) + pow(b, 2)));
    OLPoint point2 = OLPointMake(x, y);
//    OLFloat d = sqrt((point.x - point2.x) * (point.x - point2.x) + (point.y - point2.y) * (point.y - point2.y));
    OLFloat d = sqrtf((point.x - point2.x) * (point.x - point2.x) + (point.y - point2.y) * (point.y - point2.y));
    
    if (interPoint) {
        *interPoint = point2;
    }
    
    return d;
}

+ (OLFloat)lerpValueWithRenderIndex:(int)renderIndex
                    animationValues:(OLFloat *)animationValues
                 animationDurations:(OLFloat *)animationDurations
                     durationLength:(int)durationLength
                       defaultValue:(OLFloat)defaultValue {
    return [self lerpValueWithRenderIndex:renderIndex animationValues:animationValues animationDurations:animationDurations durationLength:durationLength defaultValue:defaultValue reverseAnimation:NO animationCompleted:NULL animationRepeat:NO];
}

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
                    animationRepeat:(BOOL)animationRepeat
{
    /// 动画是否完全播完
    BOOL isAnimationCompleted = NO;
    
    OLFloat value = defaultValue;
    OLFloat totalDuration = 0;
    
    /// 计算总动画时间
    int length = durationLength;
    for (int i = 0; i < length; i++) {
        totalDuration += animationDurations[i];
    }
    
    /// 每帧时间
    OLFloat msPerFrame = 1.0 / GAME_FPS; // 1 帧需要 16 毫秒
    /// 计算总动画帧数
    int totalDurationTurnCount = (int)floorf(totalDuration * GAME_FPS);
    /// 当前动画帧
    int curRenderIndex = animationRepeat ? renderIndex % totalDurationTurnCount : renderIndex;
    
    if (reverseAnimation) {// 动画倒放
        OLFloat curTime = (totalDurationTurnCount - curRenderIndex) * msPerFrame;
        
        OLFloat animationEndTime = totalDuration;// 动画结束时间
        for (int i = length - 1; i >= 0; i--) {
            /// 计算每个阶段的动画开始时间和结束时间
            OLFloat animationDuration = animationDurations[i];
            OLFloat animationStartTime = animationEndTime - animationDuration;
            
            /// 如果当前帧的时间处于阶段时间范围内，就需要做线性插值
            if (curTime >= animationStartTime && curTime <= animationEndTime) {
                OLFloat time = (animationEndTime - curTime) / animationDuration;
                OLFloat from = animationValues[i + 1];
                OLFloat to = animationValues[i];

                // 线性插值
                value = (1 - time) * from + time * to;
                break;
            }
            
            animationEndTime -= animationDuration;
        }
        
        if (curTime <= msPerFrame) {
            isAnimationCompleted = YES;
        }
        
    } else {// 动画顺放
        OLFloat curTime = curRenderIndex * msPerFrame;// 1 帧需要 16 毫秒
        
        OLFloat animationStartTime = 0;// 动画开始时间
        for (int i = 0; i < length; i++) {
            /// 计算每个阶段的动画开始时间和结束时间
            OLFloat animationDuration = animationDurations[i];
            OLFloat animationEndTime = animationStartTime + animationDuration;
            
            /// 如果当前帧的时间处于阶段时间范围内，就需要做线性插值
            if (curTime >= animationStartTime && curTime <= animationEndTime) {
                OLFloat time = (curTime - animationStartTime) / animationDuration;
                OLFloat from = animationValues[i];
                OLFloat to = animationValues[i + 1];
                // 线性插值
                value = (1 - time) * from + time * to;
                break;
            }
            
            animationStartTime += animationDuration;
        }
        
        if ((curTime + msPerFrame) >= totalDuration) {
            isAnimationCompleted = YES;
        }
    }
    
    if (animationCompleted) {
        *animationCompleted = isAnimationCompleted;
    }
    
    return value;
}

/// 点是否在 rect 里面
+ (BOOL)point:(OLPoint)point inRect:(OLRect)rect {
    return point.x >= OLRectGetMinX(rect) && point.x <= OLRectGetMaxX(rect) && point.y >= OLRectGetMinY(rect) && point.y <= OLRectGetMaxY(rect);
}

+ (OLPoint)normalizeVector:(OLPoint)vector {
    OLFloat len = sqrt(vector.x * vector.x + vector.y * vector.y);
    return OLPointMake(vector.x / len, vector.y / len);
}

+ (OLPoint)safePoint:(OLPoint)point inRect:(OLRect)rect padding:(OLSize)padding {
    OLRect mapRect = rect;
    OLRect padingRect = OLRectInset(mapRect, padding.width, padding.height);
    
    OLFloat x = MAX(point.x, padingRect.origin.x);
    x = MIN(x, OLRectGetMaxX(padingRect));
    
    OLFloat y = MAX(point.y, padingRect.origin.y);
    y = MIN(y, OLRectGetMaxY(padingRect));
    return OLPointMake(x, y);
}

// 在回字形内随机一个点
+ (OLPoint)randomPointInRect:(OLRect)rect rectPadding:(OLSize)rectPadding excludeRect:(OLRect)excludeRect excludeRectPadding:(OLSize)excludeRectPadding {
    OLRect mapRect = rect;
    OLRect padingRect = OLRectInset(mapRect, rectPadding.width, rectPadding.height);
    OLRect excludePaddingRect = OLRectInset(excludeRect, excludeRectPadding.width, excludeRectPadding.height);
    
    OLFloat left = OLRectGetMinX(padingRect);
    OLFloat right = OLRectGetMaxX(padingRect);
    OLFloat top = OLRectGetMaxY(padingRect);
    OLFloat bottom = OLRectGetMinY(padingRect);
    
    OLFloat exclude_left = OLRectGetMinX(excludePaddingRect);
    OLFloat exclude_right = OLRectGetMaxX(excludePaddingRect);
    OLFloat exclude_top = OLRectGetMaxY(excludePaddingRect);
    OLFloat exclude_bottom = OLRectGetMinY(excludePaddingRect);
    
    OLFloat x = 0;
    OLFloat y = 0;
    
    // 把回字形划分成4个区域
    NSInteger random = [MathUtils arc4random] % 4;
    if (random == 0) {// 左侧
        x = [self randomWithMin:left max:exclude_left];
        y = [self randomWithMin:bottom max:top];
    } else if (random == 1) {// 右侧
        x = [self randomWithMin:exclude_right max:right];
        y = [self randomWithMin:bottom max:top];
    } else if (random == 2) {// 上侧
        x = [self randomWithMin:left max:right];
        y = [self randomWithMin:exclude_top max:top];
    } else {// 下侧
        x = [self randomWithMin:left max:right];
        y = [self randomWithMin:bottom max:exclude_bottom];
    }
    
    return OLPointMake(x, y);
}

// 简单的线性插值
+ (OLFloat)lerpFrom:(OLFloat)startValue to:(OLFloat)endValue atTime:(OLFloat)currentTime overDuration:(OLFloat)duration {
    return startValue + (endValue - startValue) * (currentTime / duration);
}

+ (float)cos:(float)number {
    return [self truncateFloat:cosf(number) toDigits:4];
}

+ (float)sin:(float)number {
    return [self truncateFloat:sinf(number) toDigits:4];
}

+ (float)tan:(float)number {
    return [self truncateFloat:tanf(number) toDigits:4];
}

+ (float)atan2f:(float)y x:(float)x {
    return [self truncateFloat:atan2f(y, x) toDigits:4];
}

+ (float)truncateFloat:(float)number toDigits:(int)digits {
    float multiplier = powf(10.0, digits);  // 使用powf来计算幂，专门用于float
    number = floorf(number * multiplier) / multiplier;  // 使用floorf进行下取整
    return number;
}

@end
