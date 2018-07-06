//
//  MessagePersistenceService.h
//  Text2IM
//
//  Created by H2I on 2018/6/27.
//  Copyright © 2018 sliang. All rights reserved.
//

#import <Foundation/Foundation.h>

@class MessageData;

@interface MessagePersistenceService : NSObject
-(void) add:(MessageData*)messageData;
-(void) flush;
@end
