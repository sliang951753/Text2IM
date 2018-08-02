//
//  IQueryable.h
//  Text2IM
//
//  Created by H2I on 2018/7/2.
//  Copyright © 2018 sliang. All rights reserved.
//

#ifndef IQueryable_h
#define IQueryable_h
#import <Foundation/Foundation.h>

@protocol IQueryable<NSObject>

@required
- (instancetype) orderBy:(id (^) (id ))Selector;
- (instancetype) orderByDescending:(id (^) (id ))Selector;
- (NSArray*) select:(id (^) (id ))Selector;
- (instancetype) where:(bool (^) (id ))Selector;
- (id) firstOrDefault;
- (instancetype) forEach:(void (^) (id ))Handler;
- (bool) any:(bool (^) (id ))Selector;
- (bool) all:(bool (^) (id ))Selector;
- (id) toArray;
@end


#endif /* IQueryable_h */
