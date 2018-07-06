//
//  ViewController.m
//  Text2IM
//
//  Created by H2I on 2018/6/19.
//  Copyright © 2018 sliang. All rights reserved.
//

#import "ViewController.h"
#import "MessageTableView.h"
#import "MessageTableViewCell.h"
#import "Model/MessageData.h"
#import "Service/MessageReceiveService.h"
#import "Service/MessageQueryService.h"

@interface ViewController ()
@property (weak, nonatomic) IBOutlet MessageTableView *messageTableView;
@property (weak, nonatomic) IBOutlet UIBarButtonItem *syncButton;

- (IBAction)onSyncButtonClick:(id)sender;

@property (nonatomic, strong) NSMutableArray* messageDataList;
@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    self.messageDataList = [[NSMutableArray alloc] init];
}


- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


- (IBAction)onSyncButtonClick:(id)sender {
    self.syncButton.enabled = false;
    MessageReceiveService* service = [[MessageReceiveService alloc] init];
    [service receiveAllMessageData:self];
}

- (void)onReceivedAll:(NSArray<MessageData *> *)Data{
    dispatch_sync(dispatch_get_main_queue(), ^{
        self.syncButton.enabled = true;
        [self onUpdateTableView:Data];
    });
}

- (void)onReceivedOne:(MessageData *)Data {
    dispatch_sync(dispatch_get_main_queue(), ^{
        id<IQueryable> queryService = [[MessageQueryService alloc] init:self.messageDataList];
        if(![queryService any:^bool(id object) {
            MessageData* messageData = (MessageData*) object;
            if([Data equals:messageData]){
                return true;
            }
            
            return false;
        }]){
            [self.messageDataList addObject:Data];
        }
    });
}

- (nonnull UITableViewCell *)tableView:(nonnull UITableView *)tableView cellForRowAtIndexPath:(nonnull NSIndexPath *)indexPath {
    
    static NSString *CellIdentifier = @"MessagePreviewCell";
    MessageTableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier forIndexPath:indexPath];

    // Configure the cell...
    if(cell == nil)
    {
        cell = [[MessageTableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier];
    }
    
    MessageData* messageData = [self.messageDataList objectAtIndex:indexPath.row];
    
    [cell updateView:messageData];
    
    return cell;
}

- (NSInteger)tableView:(nonnull UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    
    return self.messageDataList.count;
}

- (void)onUpdateTableView:(NSArray<MessageData*> *)WithData {
    id<IQueryable> queryService = [[MessageQueryService alloc] init:self.messageDataList];
    NSArray<MessageData*>* sortedArray = [[queryService orderByDescending:^id(id object) {
        MessageData* messageData = (MessageData*) object;
        return messageData.timestamp;
    }] toArray] ;
    
    self.messageDataList = [[NSMutableArray alloc] initWithArray:sortedArray];
    
    [self.messageTableView reloadData];
}


@end
