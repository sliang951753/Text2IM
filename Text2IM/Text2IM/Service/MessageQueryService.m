//
//  MessageQueryService.m
//  Text2IM
//
//  Created by H2I on 2018/6/27.
//  Copyright © 2018 sliang. All rights reserved.
//

#import "MessageQueryService.h"

@implementation MessageQueryService

@synthesize dataSource = _dataSource;

- (instancetype)init:(NSArray<MessageData*> *)WithDataSource
{
    self = [super init];
    if (self) {
        _dataSource = [[NSArray<MessageData*> alloc] initWithArray:WithDataSource];
    }
    return self;
}

- (instancetype) orderByDescending:(id (^) (id ))Selector {
    NSArray<MessageData*> * result = [self.dataSource sortedArrayUsingComparator:^NSComparisonResult(id  _Nonnull obj1, id  _Nonnull obj2) {
        id lhs = Selector(obj2);
        id rhs = Selector(obj1);
        return [lhs compare:rhs];
    }];
    
    return [[MessageQueryService alloc] init:result];
}

- (instancetype)orderBy:(id (^) (id))Selector {
    NSArray<MessageData*> * result = [self.dataSource sortedArrayUsingComparator:^NSComparisonResult(id  _Nonnull obj1, id  _Nonnull obj2) {
        id lhs = Selector(obj1);
        id rhs = Selector(obj2);
        return [lhs compare:rhs];
    }];
    
    return [[MessageQueryService alloc] init:result];
}

- (bool)all:(bool (^)(id))Selector {
    for(int i = 0; i < self.dataSource.count; i++){
        if(Selector(self.dataSource[i]) == false)
            return false;
    }
    
    return true;
}


- (bool)any:(bool (^)(id))Selector {
    for(int i = 0; i < self.dataSource.count; i++){
        if(Selector(self.dataSource[i]))
            return true;
    }
    
    return false;
}


- (id)firstOrDefault {
    return self.dataSource.firstObject;
}


- (instancetype)forEach:(void (^)(id))Handler {
    for(int i = 0; i < self.dataSource.count; i++){
        Handler(self.dataSource[i]);
    }
    
    return self;
}

- (NSArray*)select:(id (^)(id))Selector {
    NSMutableArray* result = [[NSMutableArray alloc] initWithCapacity:self.dataSource.count];
    
    for(int i = 0; i < self.dataSource.count; i++){
        [result addObject:Selector(self.dataSource[i])];
    }
    
    return result;
}

- (instancetype)where:(bool (^)(id))Selector {
    NSMutableArray<MessageData*>* result = [[NSMutableArray<MessageData*> alloc] initWithCapacity:self.dataSource.count];
    
    for(int i = 0; i < self.dataSource.count; i++){
        MessageData* item = self.dataSource[i];
        
        if(Selector(item)){
            [result addObject:item];
        }
    }
    
    return [[MessageQueryService alloc] init:result];
}

- (id) toArray {
    return self.dataSource;
}


@end
