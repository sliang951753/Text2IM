//
//  MessageQueryService.h
//  Text2IM
//
//  Created by H2I on 2018/6/27.
//  Copyright © 2018 sliang. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Interface/IQueryable.h"
#import "../Model/MessageData.h"

@interface MessageQueryService : NSObject<IQueryable>
- (instancetype)init:(NSArray<MessageData*> *)WithDataSource;
@property (nonatomic, readonly) NSArray<MessageData*>* dataSource;
@end
