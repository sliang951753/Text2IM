//
//  ViewController.h
//  Text2IM
//
//  Created by H2I on 2018/6/19.
//  Copyright © 2018 sliang. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Service/MessageReceiveService.h"

@interface ViewController : UIViewController<MessageReceiveDelegate, UITableViewDelegate, UITableViewDataSource>
- (void) onUpdateTableView:(NSArray<MessageData*> *)WithData;
@end

