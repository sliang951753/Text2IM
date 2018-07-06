//
//  MessageData.h
//  Text2IM
//
//  Created by H2I on 2018/6/20.
//  Copyright © 2018 sliang. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MessageData : NSObject
@property (nonatomic, strong) NSString* sender;
@property (nonatomic, strong) NSString* body;
@property (nonatomic, strong) NSDate* timestamp;
@property (nonatomic, strong) NSString* publisherName;
@property (nonatomic, strong) NSString* messageGuid;

+(MessageData*)make:(NSDictionary*)fromJson;
-(bool) equals:(MessageData*)otherObject;

@end
